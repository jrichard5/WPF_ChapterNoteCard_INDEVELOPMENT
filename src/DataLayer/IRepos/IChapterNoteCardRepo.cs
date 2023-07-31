using DataLayer.Entities;

namespace DataLayer.IRepos
{
    //I feel like I could've just used IGenericRepo<Category> instead of making another, but decided to make this interfaces for entities incase they need a specific function/method in the future
    public interface IChapterNoteCardRepo : IGenericRepo<ChapterNoteCard>
    {
        Task<ChapterNoteCard> GetChapterNoteCardByTopicName(string topicName);
        Task<int> GetLastItemByTopicName(string topicName);
        Task<List<ChapterNoteCard>> GetAllChaptersWithinACategory(int categoryId);
        Task<int> GetCountByCategoryName(string categoryName);

        Task<List<ChapterNoteCard>> GetPerPageFromOneCategory(int categoryId, int page, int numberPerPage);
        Task<int> CountFromOneCategory(int categoryId);
    }
}
