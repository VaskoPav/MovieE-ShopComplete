using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MovieShop.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MovieShop.DataAccess.Repositories.EntityBaseRepository
{
    public class EntityBaseRepository<T> : IEntityBaseRepository<T> where T : class,IEntityBase, new()
    {
        private readonly MovieDbContext _dbContext;
        public EntityBaseRepository(MovieDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity= await _dbContext.Set<T>().FirstOrDefaultAsync(a => a.Id == id);
            EntityEntry entityEntry = _dbContext.Entry<T>(entity);
            entityEntry.State = EntityState.Deleted;
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return  await _dbContext.Set<T>().ToListAsync();
            
        }

        public async Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _dbContext.Set<T>();
            query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            return await query.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FirstOrDefaultAsync(a => a.Id == id);
            
        }


        public async Task UpdateAsync(int id, T entity)
        {
            EntityEntry entityEntry = _dbContext.Entry<T>(entity);
            entityEntry.State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();

        }
    }
}
