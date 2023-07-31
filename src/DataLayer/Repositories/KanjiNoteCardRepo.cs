using DataLayer.Entities;
using DataLayer.IRepos;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Repositories
{

    //Is the repository pattern suppose to be DbSets >.> monkaS i forgot
    public class KanjiNoteCardRepo : GenericRepo<KanjiNoteCard>, IKanjiNoteCardRepo
    {
        public KanjiNoteCardRepo(KanjiDbContext context) : base(context)
        {
        }

        public async Task AddAndSearchWords(KanjiNoteCard item)
        {
            var words = await _dbContext.JapaneseWordNoteCards.Where(jnc => jnc.ItemQuestion.Contains(item.ChapterNoteCard.TopicName))
                .Select(jnc => jnc.ItemQuestion)
                .ToListAsync();

            List<ChapterNoteCardSentenceNoteCard> addWords = new List<ChapterNoteCardSentenceNoteCard> ();
            foreach (var word in words)
            {
                addWords.Add(new ChapterNoteCardSentenceNoteCard { ChapterNoteCardTopicName = item.ChapterNoteCard.TopicName, SentenceNoteCardItemQuestion = word });
            }
            item.ChapterNoteCard.ChapterSentences = addWords;

            var result = _dbContext.ExtraKanjiInfos.Add(item);
            await _dbContext.SaveChangesAsync();

        }

        public async Task AddButSkipUniqueException(KanjiNoteCard item)
        {
            try
            {
                var result = await _dbContext.ExtraKanjiInfos.AddAsync(item);
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
            }
        }

        public async Task<List<KanjiNoteCard>> GetAllWithAllInfo()
        {
            return await _dbContext.ExtraKanjiInfos
                .Include(kanji => kanji.ChapterNoteCard)
                .Include(kanji => kanji.KanjiReadings)
                .AsSplitQuery()
                .ToListAsync();
        }

        public async Task BulkUpdate(List<KanjiNoteCard> items)
        {
            

            
            foreach (var item in items)
            {
                //var kanjiEntity = _dbContext.ExtraKanjiInfos.Include(knc => knc.KanjiReadings).FirstOrDefault(knc => knc.TopicName == item.TopicName);
                //if (kanjiEntity != null)
                //{
                //    _dbContext.KanjiReadings.RemoveRange(kanjiEntity.KanjiReadings);
                //}
                foreach (var kr in _dbContext.KanjiReadings.Where(kr => kr.KanjiNoteCardTopicName == item.TopicName))
                {
                    _dbContext.Remove(kr);
                }
                _dbContext.KanjiReadings.AddRange(item.KanjiReadings);
                _dbContext.ExtraKanjiInfos.Update(item);
            }
            await _dbContext.SaveChangesAsync();
        }
    }
}
