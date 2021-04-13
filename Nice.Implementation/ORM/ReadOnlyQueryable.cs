#region using
using Microsoft.EntityFrameworkCore;
using Nice.DTO;
using Nice.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
#endregion using
namespace Nice.ORM
{
    /// <summary>
    /// 默认的读写分离方案
    /// </summary>
    public class ReadOnlyQueryable<TSource> : IReadOnlyQueryable<TSource> where TSource : class, IEntitybase
    {
        #region 构造函数
        protected IQueryable<TSource> source;
        protected DbContext dbcontext;
        /// <summary>
        /// 构建一个只读的常用查询辅助类
        /// </summary>
        /// <param name="dbcontext"></param>
        /// <remarks>
        /// 1.不提供add,update,delete等方法
        /// 2.不提供状态跟踪,即默认AsNoTracking
        /// </remarks>
        public ReadOnlyQueryable(DbContext dbcontext, IQueryable<TSource> source)
        {
            this.dbcontext = dbcontext;
            this.source = source;
        }
        #endregion 构造函数

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
                NiceException.Throw("404");
            }
            return res;
        }

        public Task<TResult> SingleAsync<TResult>(int id, Expression<Func<TSource, TResult>> selector)
        {
            var res = source.Where(o => o.ID == id).Select(selector).SingleOrDefaultAsync();
            if (res == null)
            {
                NiceException.Throw("404");
            }
            return res;
        }

        public Task<TResult> SingleOrDefaultAsync<TResult>(Expression<Func<TSource, TResult>> selector)
        {
            return source.Select(selector).SingleOrDefaultAsync();
        }

        public Task<TResult> SingleOrDefaultAsync<TResult>(int id, Expression<Func<TSource, TResult>> selector)
        {
            return source.Where(o => o.ID == id).Select(selector).SingleOrDefaultAsync();
        }

        public Task<TResult> FirstOrDefaultAsync<TResult>(Expression<Func<TSource, TResult>> selector)
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

        public virtual Task<List<NamedItem>> ToItemsAsync()
        {
            return source.Select(o => new NamedItem() { ID = o.ID }).ToListAsync();
        }
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
                res.Data = await source.Skip(start).Take(request.Size).Select(selector).ToListAsync();
            }
            return res;
        }
        public Task<PageResult<TResult>> ToPageAsync<TResult>(Expression<Func<TSource, TResult>> selector)
        {
            return ToPageAsync(PageRequest.Default,selector);
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
        #endregion 其它
    }
}