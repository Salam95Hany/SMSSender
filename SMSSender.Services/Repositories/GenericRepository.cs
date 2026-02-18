using Microsoft.EntityFrameworkCore;
using SMSSender.Entities.Models;
using SMSSender.Entities.Specifications;
using SMSSender.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SMSSender.Services.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly SMSDbContext _dbContext;

        public GenericRepository(SMSDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<T>> GetAllWithSpecAsync(ISpecification<T> spec)
        {
            return await ApplySecifications(spec).ToListAsync();
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _dbContext.Set<T>().AsNoTracking().ToListAsync();
        }

        public async Task<T?> GetByIdWithSpecAsync(ISpecification<T> spec)
        {
            return await ApplySecifications(spec).FirstOrDefaultAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(new object[] { id });
        }

        public async Task<T?> GetByIdAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbContext.Set<T>().FirstOrDefaultAsync(predicate);
        }

        public async Task AddAsync(T entity)
        => await _dbContext.Set<T>().AddAsync(entity);

        public void Update(T entity)
        => _dbContext.Set<T>().Update(entity);

        public void Delete(T entity)
        => _dbContext.Set<T>().Remove(entity);

        public void DeleteRange(IEnumerable<T> entities)
        => _dbContext.Set<T>().RemoveRange(entities);

        public async Task DeleteWhereAsync(Expression<Func<T, bool>> predicate)
        => await _dbContext.Set<T>().Where(predicate).ExecuteDeleteAsync();

        private IQueryable<T> ApplySecifications(ISpecification<T> spec)
        {
            return SpecificationsEvaluator<T>.GetQuery(_dbContext.Set<T>(), spec);
        }

        public Task<int> CountAsync(Expression<Func<T, bool>> predicate)
        {
            return _dbContext.Set<T>().CountAsync(predicate);
        }

        public Task<int> CountAsync()
        {
            return _dbContext.Set<T>().CountAsync();
        }

        public Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return _dbContext.Set<T>().FirstOrDefaultAsync(predicate); // => indexer.name == "x"
        }

        public async Task<int> GetCountAsync(ISpecification<T> spec)
        {
            return await ApplySecifications(spec).CountAsync();
        }

        public async Task<List<T>> GetAllAsQueryableAsync(ISpecification<T>? spec = null)
        {
            var query = _dbContext.Set<T>().AsQueryable();

            if (spec != null)
            {
                query = SpecificationsEvaluator<T>.GetQuery(query, spec);
            }

            return await query.ToListAsync();
        }

        public IQueryable<T> GetAllAsQueryable()
        {
            return _dbContext.Set<T>().AsQueryable();
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        => await _dbContext.Set<T>().AddRangeAsync(entities);

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        => await _dbContext.Set<T>().AnyAsync(predicate);

        public async Task<bool> AnyAsync()
        => await _dbContext.Set<T>().AnyAsync();

        public async Task<TResult> MaxAsync<TResult>(Expression<Func<T, TResult>> selector)
        {
            return await _dbContext.Set<T>().MaxAsync(selector);
        }
        public IQueryable<T> Where(Expression<Func<T, bool>> predicate)
        {
            return _dbContext.Set<T>().Where(predicate);
        }

        public async Task<List<T>> WhereAsync(Expression<Func<T, bool>> predicate, int? take = null)
        {
            var query = _dbContext.Set<T>().Where(predicate);

            if (take.HasValue)
                query = query.Take(take.Value);

            return await query.ToListAsync();
        }

        public async Task<T?> GetByIdWithIncludeAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IQueryable<T>> include)
        {
            IQueryable<T> query = _dbContext.Set<T>();

            if (include != null)
                query = include(query);

            return await query.FirstOrDefaultAsync(predicate);
        }
        public async Task<List<T>> GetAllWithIncludeAsync(Func<IQueryable<T>, IQueryable<T>> include)
        {
            IQueryable<T> query = _dbContext.Set<T>();

            if (include != null)
                query = include(query);

            return await query.ToListAsync();
        }
    }
}
