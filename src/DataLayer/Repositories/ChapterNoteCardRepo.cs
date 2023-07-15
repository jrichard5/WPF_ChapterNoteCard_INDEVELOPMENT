﻿using DataLayer.Entities;
using DataLayer.IRepos;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Repositories
{
    public class ChapterNoteCardRepo : GenericRepo<ChapterNoteCard>, IChapterNoteCardRepo
    {
        //Is the repository pattern suppose to be DbSets >.> monkaS i forgot
        //private readonly DbSet<ChapterNoteCard> _chapters;

        public ChapterNoteCardRepo(KanjiDbContext context) : base(context)
        {
            //I think this is what I would do if using dbset....
            //_chapters = context.Chapters;
        }

        public async Task<List<ChapterNoteCard>> GetAllChaptersWithinACategory(int categoryId)
        {
            return await _dbContext.Chapters.Where(chap => chap.CategoryId == categoryId).ToListAsync();
        }

        public async Task<ChapterNoteCard> GetChapterNoteCardByTopicName(string topicName)
        {
            return await _dbContext.Chapters.FirstAsync(chapter => chapter.TopicName == topicName);
        }

        public async Task<int> GetCountByCategoryName(string categoryName)
        {
            var count = await _dbContext.Chapters.Where(cnc => cnc.Category.CategoryName == categoryName).CountAsync();
            return count;
        }

        public async Task<int> GetLastItemByTopicName(string topicName)
        {
            var lastItem = await _dbContext.Chapters.Where(c => c.TopicName == topicName)
                .SelectMany(c => c.ChapterSentences)
                .MaxAsync(cs => (int?)cs.ExtraJishoInfo.Order);

            if (lastItem == null)
            {
                return 0;
            }
            Console.WriteLine();
            return (int)lastItem;
        }
    }
}
