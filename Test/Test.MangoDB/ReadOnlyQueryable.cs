using MongoDB.Driver;
using Nice.DTO;
using Nice.ORM;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Test.MangoDB
{
    public class ReadOnlyQueryable<TEntity> : IReadOnlyQueryable<TEntity>
    {
        public Task<bool> AnyAsync()
        {
            throw new NotImplementedException();
        }

        public Task<int> CountAsync()
        {
            throw new NotImplementedException();
        }

        public Task<TResult> FirstOrDefaultAsync<TResult>(Expression<Func<TEntity, TResult>> selector)
        {
            throw new NotImplementedException();
        }

        public Task<TResult> SingleAsync<TResult>(Expression<Func<TEntity, TResult>> selector)
        {
            throw new NotImplementedException();
        }

        public Task<TResult> SingleAsync<TResult>(int id, Expression<Func<TEntity, TResult>> selector)
        {
            throw new NotImplementedException();
        }

        public Task<TResult> SingleOrDefaultAsync<TResult>(Expression<Func<TEntity, TResult>> selector)
        {
            throw new NotImplementedException();
        }

        public Task<TResult> SingleOrDefaultAsync<TResult>(int id, Expression<Func<TEntity, TResult>> selector)
        {
            throw new NotImplementedException();
        }

        public Task<TResult[]> ToArrayAsync<TResult>(Expression<Func<TEntity, TResult>> selector)
        {
            throw new NotImplementedException();
        }

        public Task<List<NamedItem>> ToItemsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<TResult>> ToListAsync<TResult>(Expression<Func<TEntity, TResult>> selector)
        {
            throw new NotImplementedException();
        }

        public Task<PageResult<TResult>> ToPageAsync<TResult>(PageRequest request, Expression<Func<TEntity, TResult>> selector)
        {
            throw new NotImplementedException();
        }

        public Task<PageResult<TResult>> ToPageAsync<TResult>(Expression<Func<TEntity, TResult>> selector)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate)
        {
            throw new NotImplementedException();
        }
    }
}
