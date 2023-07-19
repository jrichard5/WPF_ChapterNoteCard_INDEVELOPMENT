using CsvHelper.Configuration.Attributes;
using DataLayer.Entities;
using DataLayer.IRepos;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Repositories
{

    //Is the repository pattern suppose to be DbSets >.> monkaS i forgot
    public class JapaneseWordNoteCardRepo : GenericRepo<JapaneseWordNoteCard>, IJapaneseWordNoteCardRepo
    {
        public JapaneseWordNoteCardRepo(KanjiDbContext context) : base(context) { }

        public async Task AddAsync(List<JapaneseWordNoteCard> cards)
        {
            var cardsTopName = cards.Select(c => c.SentenceNoteCard.ItemQuestion).ToList();
            var addedCards = await _dbContext.JapaneseWordNoteCards.Where(dbcard => cardsTopName.Contains(dbcard.ItemQuestion)).ToListAsync();
            var cardsToAdd = cards.RemoveAll(c => addedCards.Any(added => added.ItemQuestion == c.SentenceNoteCard.ItemQuestion));

            foreach (var card in cards)
            {
                _dbContext.JapaneseWordNoteCards.Add(card);
            }
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<JapaneseWordNoteCard>> GetAllFromOneCategory(string topicName)
        {
            var wordswithChapter = await _dbContext.JapaneseWordNoteCards.Where(jnc => jnc.SentenceNoteCard.Chapters.Any(c => c.TopicName == topicName))
                .Include(j => j.SentenceNoteCard)
                .Include(j => j.SentenceNoteCard.ChapterSentences)
                .ThenInclude(cs => cs.ExtraJishoInfo)
                .AsSplitQuery()
                .ToListAsync();

            return wordswithChapter;
        }
        public async Task BulkUpdate(List<JapaneseWordNoteCard> entities)
        {
            foreach (var entity in entities)
            {
                //_dbContext.Sentences.Attach(entity.SentenceNoteCard);
                //_dbContext.ChapterSentences.Attach(entity.SentenceNoteCard.ChapterSentences.First());
                //_dbContext.ExtraJishoInfos.Attach(entity.SentenceNoteCard.ChapterSentences.First().ExtraJishoInfo);
                 this._dbContext.JapaneseWordNoteCards.Update(entity);
            }
            await this._dbContext.SaveChangesAsync();
        }

        public async Task<int> CountFromOneChapter(string topicName)
        {
            var count = await _dbContext.JapaneseWordNoteCards.Where(jnc => jnc.SentenceNoteCard.Chapters.Any(c => c.TopicName == topicName)).CountAsync();
            return count;
        }

        public async Task<List<JapaneseWordNoteCard>> GetPerPageFromOneCategory(string topicName, int page, int numberPerPage)
        {
            var wordswithChapter = await _dbContext.JapaneseWordNoteCards.Where(jnc => jnc.SentenceNoteCard.Chapters.Any(c => c.TopicName == topicName))
                .Include(j => j.SentenceNoteCard)
                .Include(j => j.SentenceNoteCard.ChapterSentences)
                .ThenInclude(cs => cs.ExtraJishoInfo)
                .AsSplitQuery()
                .Skip((page-1) * numberPerPage).Take(numberPerPage)
                .ToListAsync();

            return wordswithChapter;
        }

        public async Task<Dictionary<string, bool[]>> GetCharacterExistFromList(IList<string> listOfStrings)
        {
            var dictionary = new Dictionary<string, bool[] >();

            //Get all characters and search database for ones that don't exist
            var charInsideList = new List<string>();
            foreach (var str in listOfStrings)
            {
                var result = str.ToArray().Select(c => c.ToString());
                charInsideList.AddRange(result);
            }
            
            foreach (var str in listOfStrings)
            {
                var existPosition = new bool[str.Length];
                await ForEachCharInString(str, existPosition, charInsideList);
                await CheckForHiraganaInHint(str, existPosition);
                dictionary.Add(str, existPosition);
            }
            return dictionary;
        }

        private async Task CheckForHiraganaInHint(string str, bool[] existPosition)
        {
            var hint = await _dbContext.JapaneseWordNoteCards.Where(jnc => jnc.ItemQuestion == str).Select(jnc => jnc.SentenceNoteCard.Hint).FirstOrDefaultAsync();
            for(int i = 0; i <str.Length; i++)
            {
                if (hint.Contains(str[i]))
                {
                    existPosition[i] = true;
                }
            }
        }

        private async Task ForEachCharInString(string str, bool[] existPosition, IList<string> charInsideList)
        {
            var doesExistList =  await _dbContext.ExtraKanjiInfos.Where(knc => charInsideList.Contains(knc.TopicName)).Select(knc => knc.TopicName).ToListAsync();
            foreach (var single in str)
            {
                var result = doesExistList.Contains(single.ToString());
                if (result)
                {
                    var foundIndexes = new List<int>();
                    for (int i = 0; i < str.Length; i++)
                    {
                        if (str[i] == single) { foundIndexes.Add(i); }
                    }


                    foreach (var foundIndex in foundIndexes)
                    {
                        existPosition[foundIndex] = true;
                    }
                }

            }
        }

        public async Task AddAndAddBridgeTableInfo(JapaneseWordNoteCard card)
        {
            //Takes the word and return kanji that are inside the word
            var kanjis = await _dbContext.ExtraKanjiInfos.Where(knc => card.ItemQuestion.Contains(knc.TopicName))
                //The card already has a parent attached to it, so remove that one.
                .Where(knc => knc.ChapterNoteCard.TopicName != card.SentenceNoteCard.ChapterSentences.First().ChapterNoteCardTopicName)
            .Select(knc => knc.TopicName)
            .ToListAsync();

            List<ChapterNoteCardSentenceNoteCard> extraKanjiToAdd = new List<ChapterNoteCardSentenceNoteCard>();
            foreach (var kanji in kanjis)
            {
                extraKanjiToAdd.Add(new ChapterNoteCardSentenceNoteCard
                {
                    ChapterNoteCardTopicName = kanji,
                    SentenceNoteCardItemQuestion = card.ItemQuestion,
                    ExtraJishoInfo = new ExtraJishoInfoOnBridge
                    {
                        Order = 0,
                        PageNumber = 0,
                    }
                }) ;
            }
            card.SentenceNoteCard.ChapterSentences.AddRange(extraKanjiToAdd);

            var result = _dbContext.JapaneseWordNoteCards.Add(card);
            await _dbContext.SaveChangesAsync();
        }
    }
}






//Old Comments

//Didn'tWork put ill keep in
//EF has trouble with Where and any???.  Need to make a "Built expression"https://stackoverflow.com/questions/68737681/the-linq-expression-could-not-be-translated-either-rewrite-the-query-in-a-form
// EF do not supports complex predicates with local collections and here you need to build expression tree dynamically  https://stackoverflow.com/questions/71043307/find-in-entity-framework-multiple-or-parameters
//I thihnk i need another library, but im lazy so im going to send 8 hours trying to figure out another way


//something that i remembered during internship, and it helped me firgure it out
//filter option? maybe https://stackoverflow.com/questions/33153932/filter-search-using-multiple-fields-asp-net-mvc

//var query = _dbContext.JapaneseWordNoteCards.AsQueryable();
//foreach (var card in cards)
//{
//This filter the list by one, then returned only one.  then it filtered it by two, but since it was one, it got filtered otu
//    query = query.Where(dbcard => dbcard.SentenceNoteCard.ItemQuestion == card.SentenceNoteCard.ItemQuestion);
//query  =  query.Where(dbentry => ArrayOfMultipleDifferentFilterValues.Contains(dbentry.FilterOptionColumn)
//query = query.Where(dbentry => SecondArrayOfMultipleDifferntFilterValues.Contains(debentry.DifferentColumn);
//}
//var qeurycards = query.ToList();
