using Nice.DTO;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Nice.ORM
{
    /// <summary>
    /// 只读ORM统一接口
    /// </summary>
    public interface IReadOnlyQueryable<TSource>
    {
        //    /// <summary>
        //    /// 设置一个Selector
        //    /// </summary>
        //    /// <typeparam name="TResult"></typeparam>
        //    /// <param name="selector"></param>
        //    /// <returns></returns>
        //    INiceQueryable<TResult> Select<TResult>(Expression<Func<TSource, TResult>> selector);
        /// <summary>
        /// 查询数据集并转换为List
        /// </summary>
        /// <returns></returns>
        Task<List<TResult>> ToListAsync<TResult>(Expression<Func<TSource, TResult>> selector);
        /// <summary>
        /// 查询数据集并转换为Array
        /// </summary>
        /// <returns></returns>
        Task<TResult[]> ToArrayAsync<TResult>(Expression<Func<TSource, TResult>> selector);
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<PageResult<TResult>> ToPageAsync<TResult>(PageRequest request, Expression<Func<TSource, TResult>> selector);
        /// <summary>
        /// 使用默认的分页设置进行分页查询  
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="selector"></param>
        /// <returns></returns>
        Task<PageResult<TResult>> ToPageAsync<TResult>(Expression<Func<TSource, TResult>> selector);

        /// <summary>
        /// 查询数据集,并转换为List<NamedItem>,专供下拉框使用
        /// </summary>
        Task<List<NamedItem>> ToItemsAsync();

        /// <summary>
        /// 增加一个Where条件
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IReadOnlyQueryable<TSource> Where(Expression<Func<TSource, bool>> predicate);
        /// <summary>
        /// 查询单条数据,如果没查到[   会   ]抛出404异常
        /// </summary>
        /// <returns></returns>
        /// <exception cref="404"/>
        Task<TResult> SingleAsync<TResult>(Expression<Func<TSource, TResult>> selector);
        /// <summary>
        /// 查询单条数据,如果没查到[   不会   ]抛出404异常
        /// </summary>
        /// <returns></returns>
        Task<TResult> SingleOrDefaultAsync<TResult>(Expression<Func<TSource, TResult>> selector);
        /// <summary>
        /// 根据ID查询单条数据,如果没查到[   不会   ]抛出404异常
        /// </summary>
        /// <returns></returns>
        Task<TResult> SingleOrDefaultAsync<TResult>(int id, Expression<Func<TSource, TResult>> selector);
        /// <summary>
        /// 根据ID查询单条数据,如果没查到[   会   ]抛出404异常
        /// </summary>
        /// <returns></returns>
        /// <exception cref="404"/>
        Task<TResult> SingleAsync<TResult>(int id, Expression<Func<TSource, TResult>> selector);
        /// <summary>
        /// 查询符合条件的第一条数据
        /// </summary>
        /// <returns></returns>
        /// <remarks>请尽量使用SingleOrDefaultAsync</remarks>
        [Obsolete("请尽量使用SingleOrDefaultAsync;如果一定要使用此方法,请确保带够查询条件,否则极易出现难以修复的错误数据;使用此方法的代码必须通过注释详细描述原因,在代码审核时将被特别关注,请珍惜您的绩效")]
        Task<TResult> FirstOrDefaultAsync<TResult>(Expression<Func<TSource, TResult>> selector);
        /// <summary>
        /// 统计符合查询条件的总数
        /// </summary>
        /// <returns></returns>
        Task<int> CountAsync();
        /// <summary>
        /// 是否存在符合查询条件的数据
        /// </summary>
        /// <returns></returns>
        Task<bool> AnyAsync();
    }
}