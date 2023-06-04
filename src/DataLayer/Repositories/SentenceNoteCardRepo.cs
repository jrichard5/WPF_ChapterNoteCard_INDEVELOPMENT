using DataLayer.Entities;
using DataLayer.IRepos;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Repositories
{
    public class SentenceNoteCardRepo : GenericRepo<SentenceNoteCard>, ISentenceNoteCardRepo
    {

        //Is the repository pattern suppose to be DbSets >.> monkaS i forgot
        public SentenceNoteCardRepo(KanjiDbContext context) : base(context) { }

        public async Task<List<SentenceNoteCard>> GetAllWithAChapter(string topicName)
        {
            var sentenceCards = await _dbContext.Sentences.Where(sc => sc.Chapters.Any(c => c.TopicName == topicName))
                .ToListAsync();
            return sentenceCards;
        }
    }
}
