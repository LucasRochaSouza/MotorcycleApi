using Domain.Entities;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Abstractions
{
    public abstract class BaseRepository<T> where T : BaseEntity
    {
        private readonly DatabaseContext _databaseContext;

        protected readonly DbSet<T> _dbSet;

        public BaseRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
            _dbSet = _databaseContext.Set<T>();
        }

        public Task<List<T>> GetAll()
        {
            return _dbSet.ToListAsync();
        }

        public Task Create(T entity)
        {
            _databaseContext.Set<T>().AddAsync(entity);

            return _databaseContext.SaveChangesAsync();
        }

        public Task<T> GetBydId(long id)
        {
            return _dbSet.Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public Task<int> Delete(long id)
        {
            return _databaseContext.Set<T>().Where(x => x.Id == id).ExecuteDeleteAsync();
        }

        public Task Update(T entity)
        {
            _databaseContext.Set<T>().Attach(entity);
            _databaseContext.Entry(entity).State = EntityState.Modified;

            return _databaseContext.SaveChangesAsync();
        }
    }
}
