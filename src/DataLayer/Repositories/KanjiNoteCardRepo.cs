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
    }
}
