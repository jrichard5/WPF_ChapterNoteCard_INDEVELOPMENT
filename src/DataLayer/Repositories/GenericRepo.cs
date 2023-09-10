using DataLayer.IRepos;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Repositories
{
    //Is the repository pattern suppose to be DbSets >.> monkaS i forgot
    public class GenericRepo<T> : IGenericRepo<T> where T : class
    {
        public readonly KanjiDbContext _dbContext;

        public GenericRepo(KanjiDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<T> AddAsync(T entity)
        {
            var result = await this._dbContext.Set<T>().AddAsync(entity);
            await this._dbContext.SaveChangesAsync();
            return result.Entity;
        }

        

        public async Task DeleteAsync(T entity)
        {
            this._dbContext.Set<T>().Remove(entity);
            await this._dbContext.SaveChangesAsync();
        }

        public async Task DeleteByList(IList<T> entity)
        {
            foreach (var item in entity)
            {
                _dbContext.Set<T>().Attach(item);
                _dbContext.Set<T>().Remove(item);
            }
            await this._dbContext.SaveChangesAsync();
        }

        public async Task DeleteWithAttach(T entity)
        {
            _dbContext.Set<T>().Attach(entity);
            _dbContext.Set<T>().Remove(entity);
            await this._dbContext.SaveChangesAsync();
        }

        public async Task<T> FindByIdAsync(int id)
        {
            var entity = await this._dbContext.Set<T>().FindAsync(id);
            return entity;
        }

        public async Task<List<T>> GetAll()
        {
            var entities = await this._dbContext.Set<T>().ToListAsync();
            return entities;
        }

        public async Task BulkUpdateGeneric(List<T> entities)
        {
            foreach (var entity in entities)
            {
                this._dbContext.Set<T>().Update(entity);
            }
            await this._dbContext.SaveChangesAsync();
        }

        public async Task UpdateGeneric(T entity)
        {
            this._dbContext.Set<T>().Update(entity);
            await this._dbContext.SaveChangesAsync();
        }
    }
}
