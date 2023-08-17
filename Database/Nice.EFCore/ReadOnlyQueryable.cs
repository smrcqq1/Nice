#region using
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Nice.DataProvider;
using Nice.DTO;
using System.Linq.Expressions;
#endregion using
namespace Nice.EFCore
{
    /// <summary>
    /// EFCore的只读方案
    /// </summary>
    internal class ReadOnlyQueryable<TSource> : IReadOnlyQueryable<TSource>
    {
        #region 构造函数
        internal System.Linq.IQueryable<TSource> source;
        internal DbContext dbcontext;
        /// <summary>
        /// 构建一个只读的常用查询辅助类
        /// </summary>
        /// <param name="dbcontext"></param>
        /// <remarks>
        /// 1.不提供add,update,delete等方法
        /// 2.不提供状态跟踪,即默认AsNoTracking
        /// </remarks>
        public ReadOnlyQueryable(DbContext dbcontext, System.Linq.IQueryable<TSource> source)
        {
            this.dbcontext = dbcontext;
            this.source = source;
        }
        #endregion 构造函数

        #region join

        public IReadOnlyQueryable<JoinResult<TSource, TInner>> Join<TInner>(Expression<Func<TSource, Guid>> outerKeySelector, Expression<Func<TInner, Guid>> innerKeySelector) where TInner : class
        {
            var newSource = source.Join(dbcontext.Set<TInner>(), outerKeySelector, innerKeySelector, (a, b) => new JoinResult<TSource, TInner>() { Inner = a, Outer = b });
            return new ReadOnlyQueryable<JoinResult<TSource, TInner>>(dbcontext, newSource);
        }
        public IReadOnlyQueryable<TResult> Join<TInner, TKey, TResult>(Expression<Func<TSource, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TSource, TInner, TResult>> resultSelector) where TInner : class
        {
            var newSource = source.Join(dbcontext.Set<TInner>(), outerKeySelector, innerKeySelector, resultSelector);
            return new ReadOnlyQueryable<TResult>(dbcontext,newSource);
        }
        public IReadOnlyQueryable<TResult> GroupJoin<TInner, TKey, TResult>(Expression<Func<TSource, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TSource, IEnumerable<TInner>, TResult>> resultSelector) where TInner : class
        {
            var newSource = source.GroupJoin(dbcontext.Set<TInner>(), outerKeySelector, innerKeySelector, resultSelector);
            return new ReadOnlyQueryable<TResult>(dbcontext, newSource);
        }
        public IReadOnlyQueryable<TResult> LeftJoin<TInner, TKey, TResult>(Expression<Func<TSource, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TSource, TInner, TResult>> resultSelector) where TInner : class
        {
#pragma warning disable EF1001 // Internal EF Core API usage.
            var newSource = source.LeftJoin(dbcontext.Set<TInner>(), outerKeySelector, innerKeySelector, resultSelector);
#pragma warning restore EF1001 // Internal EF Core API usage.
            return new ReadOnlyQueryable<TResult>(dbcontext, newSource);
        }
        public IReadOnlyQueryable<IGrouping<TKey, TSource>> GroupBy<TKey>(Expression<Func<TSource, TKey>> keySelector)
        {
            var newSource = source.GroupBy(keySelector);
            return new ReadOnlyQueryable<IGrouping<TKey, TSource>>(dbcontext, newSource);
        }
        #endregion join

        #region Select
        //不提供单独的Select,是为了避免业务开发人员不使用Select,直接查询整个模型
        //public INiceQueryable<TResult> Select<TResult>(Expression<Func<TSource, TResult>> selector)
        //{
        //    return new NiceQueryable<TResult>(dbcontext,source.Select(selector));
        //}
        #endregion Select

        #region 单条数据查询
        public async Task<TResult> SingleAsync<TResult>(Expression<Func<TSource, TResult>> selector)
        {
            var res = await source.Select(selector).SingleOrDefaultAsync();
            if (res == null)
            {
                throw new BuzinessException("404");
            }
            return res;
        }

        //public Task<TResult> SingleAsync<TResult>(int id, Expression<Func<TSource, TResult>> selector)
        //{
        //    var res = source.Where(o => o.ID == id).Select(selector).SingleOrDefaultAsync();
        //    if (res == null)
        //    {
        //        //NiceException.Throw("404");
        //    }
        //    return res;
        //}

        public async Task<TResult?> SingleOrDefaultAsync<TResult>(Expression<Func<TSource, TResult>> selector)
        {
            var _source = source.Select(selector);
            var res = await _source.SingleOrDefaultAsync();
            return res;
        }

        //public Task<TResult> SingleOrDefaultAsync<TResult>(int id, Expression<Func<TSource, TResult>> selector)
        //{                                                                                                                                                                                                                      
        //    return source.Where(o => o.ID == id).Select(selector).SingleOrDefaultAsync();
        //}

        public Task<TResult?> FirstOrDefaultAsync<TResult>(Expression<Func<TSource, TResult>> selector)
        {
            return source.Select(selector).FirstOrDefaultAsync();
        }
        #endregion 单条数据查询

        #region Where
        public virtual IReadOnlyQueryable<TSource> Where(Expression<Func<TSource, bool>> predicate)
        {
            source = source.Where(predicate);
            return this;
        }
        public IReadOnlyQueryable<TSource> WhereIf(bool condition, Expression<Func<TSource, bool>> expression)
        {
            if (condition)
            {
                return Where(expression);
            }
            return this;
        }

        public IReadOnlyQueryable<TSource> WhereIf(string condition, Expression<Func<TSource, bool>> expression)
        {
            return WhereIf(!string.IsNullOrEmpty(condition), expression);
        }

        public IReadOnlyQueryable<TSource> WhereIf(Guid? condition, Expression<Func<TSource, bool>> expression)
        {
            return WhereIf(condition != null && condition != Guid.Empty, expression);
        }
        #endregion Where

        #region 列表查询
        public Task<TResult[]> ToArrayAsync<TResult>(Expression<Func<TSource, TResult>> selector)
        {
            return source.Select(selector).ToArrayAsync();
        }

        public Task<List<TResult>> ToListAsync<TResult>(Expression<Func<TSource, TResult>> selector)
        {
            return source.Select(selector).ToListAsync();
        }

        //public virtual Task<List<NamedItem>> ToItemsAsync()
        //{
        //    return source.Select(o => new NamedItem() { ID = o.ID }).ToListAsync();
        //}
        #endregion 列表查询

        #region 分页查询
        public async Task<PageResult<TResult>> ToPageAsync<TResult>(PageRequest request, Expression<Func<TSource, TResult>> selector)
        {
            var res = new PageResult<TResult>
            {
                Total = await source.CountAsync()
            };

            if (request.Index < 1)
            {
                request.Index = 1;
            }
            if (request.Size < 1)
            {
                request.Size = 10;
            }
            int start = (request.Index - 1) * request.Size;
            if (res.Total > start)
            {
                res.Data = await source
                    .OrderByDescending(o=> (o as IDataTable)!.CreateTime)
                    .Skip(start).Take(request.Size).Select(selector).ToListAsync();
            }
            return res;
        }
        public Task<PageResult<TResult>> PageAsync<TResult>(Expression<Func<TSource, TResult>> selector)
        {
            return ToPageAsync(PageRequest.Default, selector);
        }
        #endregion 分页查询

        #region 其它
        public Task<int> CountAsync()
        {
            return source.CountAsync();
        }

        public Task<bool> AnyAsync()
        {
            return source.AnyAsync();
        }

        public async Task<TResult?> SingleOrDefaultAsync<TResult>(Guid id, Expression<Func<TSource, TResult>> selector)
        {
            Where(o => (o as IDataTable)!.Id == id);
            var res = await SingleOrDefaultAsync(selector);
            return res;
        }

        public async Task<TResult> SingleAsync<TResult>(Guid id, Expression<Func<TSource, TResult>> selector)
        {
            Where(o => (o as IDataTable)!.Id == id);
            var res = await SingleOrDefaultAsync(selector);
            if (res == null)
            {
                throw new BuzinessException("未找到指定Id的数据");
            }
            return res;
        }
        #endregion 其它

        #region 排序
        public virtual IReadOnlyQueryable<TSource> OrderBy<TProperty>(Expression<Func<TSource, TProperty>> predicate)
        {
            source = source.OrderBy(predicate);
            return this;
        }
        public virtual IReadOnlyQueryable<TSource> OrderByDesc<TProperty>(Expression<Func<TSource, TProperty>> predicate)
        {
            source = source.OrderByDescending(predicate);
            return this;
        }
        #endregion 排序
    }
}