using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Nice.ORM
{
    public interface IReadWriteQueryable<TSource>: IReadOnlyQueryable<TSource>
    {
        /// <summary>
        /// 增加一个Where条件
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        new IReadWriteQueryable<TSource> Where(Expression<Func<TSource, bool>> predicate);
        /// <summary>
        /// 新增单条数据,会自动提交
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        Task<bool> AddAsync(TSource data);
        /// <summary>
        /// 批量新增数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        Task<bool> AddAsync(IEnumerable<TSource> data);
        /// <summary>
        /// 删除指定ID的数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync(int id);
        /// <summary>
        /// 删除符合查询条件的所有数据
        /// </summary>
        /// <returns></returns>
        Task<bool> DeleteAsync();
        /// <summary>
        /// 删除指定的数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync(TSource data);
        /// <summary>
        /// 批量删除数据
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync(IEnumerable<TSource> datas);
        /// <summary>
        /// 对符合查询条件的所有数据执行指定的更新操作
        /// </summary>
        /// <param name="updateExpression">更新表达式</param>
        /// <returns></returns>
        Task<bool> UpdateAsync(Expression<Func<TSource, TSource>> updateExpression);
        /// <summary>
        /// 对指定ID的数据执行指定的更新操作
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updateExpression"></param>
        /// <returns></returns>
        Task<bool> UpdateAsync(int id, Expression<Func<TSource, TSource>> updateExpression);
    }
}