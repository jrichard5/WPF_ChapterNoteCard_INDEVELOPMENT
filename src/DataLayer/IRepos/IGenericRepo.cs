namespace DataLayer.IRepos
{
    public interface IGenericRepo<T> where T : class
    {
        public Task<T> AddAsync(T entity);
        public Task DeleteAsync(T entity);
        public Task<T> FindByIdAsync(int id);
        public Task DeleteByList(IList<T> entities);
        public Task<List<T>> GetAll();
        public Task DeleteWithAttach(T entity);
        public Task BulkUpdateGeneric(List<T> entities);
    }
}
