using DataLayer.Entities;

namespace DataLayer.IRepos
{
    //I feel like I could've just used IGenericRepo<Category> instead of making another, but decided to make this interfaces for entities incase they need a specific function/method in the future
    public interface IJapaneseWordNoteCardRepo : IGenericRepo<JapaneseWordNoteCard>
    {
        Task AddAsync(List<JapaneseWordNoteCard> cards);

        Task AddAndAddBridgeTableInfo(JapaneseWordNoteCard card);

        Task<List<JapaneseWordNoteCard>> GetAllFromOneCategory(string topicName);

        Task BulkUpdate(List<JapaneseWordNoteCard> cards);

        Task<int> CountFromOneChapter(string topicName);

        Task<List<JapaneseWordNoteCard>> GetPerPageFromOneCategory(string topicName, int page, int numberPerPage);

        Task<Dictionary<string, bool[]>> GetCharacterExistFromList(IList<string> listOfStrings);
    }
}
