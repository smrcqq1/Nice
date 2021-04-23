#region using
using Nice.DTO;
using Nice.ORM;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
#endregion using
namespace Nice.Redis
{
    /// <summary>
    /// Redis版的数据只读辅助类库
    /// </summary>
    public class ReadOnlyQueryable<TSource> : IReadOnlyQueryable<TSource>
    {
        public Task<bool> AnyAsync()
        {
            throw new NotImplementedException();
        }

        public Task<int> CountAsync()
        {
            throw new NotImplementedException();
        }

        public Task<TResult> FirstOrDefaultAsync<TResult>(Expression<Func<TSource, TResult>> selector)
        {
            throw new NotImplementedException();
        }

        public Task<TResult> SingleAsync<TResult>(Expression<Func<TSource, TResult>> selector)
        {
            throw new NotImplementedException();
        }

        public Task<TResult> SingleAsync<TResult>(int id, Expression<Func<TSource, TResult>> selector)
        {
            throw new NotImplementedException();
        }

        public Task<TResult> SingleOrDefaultAsync<TResult>(Expression<Func<TSource, TResult>> selector)
        {
            throw new NotImplementedException();
        }

        public Task<TResult> SingleOrDefaultAsync<TResult>(int id, Expression<Func<TSource, TResult>> selector)
        {
            throw new NotImplementedException();
        }

        public Task<TResult[]> ToArrayAsync<TResult>(Expression<Func<TSource, TResult>> selector)
        {
            throw new NotImplementedException();
        }

        public Task<List<NamedItem>> ToItemsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<TResult>> ToListAsync<TResult>(Expression<Func<TSource, TResult>> selector)
        {
            throw new NotImplementedException();
        }

        public Task<PageResult<TResult>> ToPageAsync<TResult>(PageRequest request, Expression<Func<TSource, TResult>> selector)
        {
            throw new NotImplementedException();
        }

        public Task<PageResult<TResult>> ToPageAsync<TResult>(Expression<Func<TSource, TResult>> selector)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyQueryable<TSource> Where(Expression<Func<TSource, bool>> predicate)
        {
            throw new NotImplementedException();
        }
    }
}