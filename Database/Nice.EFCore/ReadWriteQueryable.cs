#region using
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Nice.DataProvider;
using System.Linq.Expressions;
using System.Transactions;
#endregion using
namespace Nice.EFCore
{
    /// <summary>
    /// 可读可写的数据库查询封装类
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <remarks>1</remarks>
    internal class ReadWriteQueryable<TSource> : ReadOnlyQueryable<TSource>,IReadWriteQueryable<TSource>
    {
        #region 构造函数
        /// <summary>
        /// 构建一个可读可写的常用查询辅助类
        /// </summary>
        /// <param name="dbcontext"></param>
        /// <remarks>
        /// 1.提供add,update,delete等方法
        /// 2.提供状态跟踪
        /// </remarks>
        public ReadWriteQueryable(DbContext dbcontext, IQueryable<TSource> source) :base(dbcontext, source)
        {
        }
        #endregion 构造函数

        #region join
        public new IReadOnlyQueryable<JoinResult<TSource, TInner>> Join<TInner>(Expression<Func<TSource, Guid>> outerKeySelector, Expression<Func<TInner, Guid>> innerKeySelector) where TInner : class
        {
            var newSource = source.Join(dbcontext.Set<TInner>(), outerKeySelector, innerKeySelector, (a, b) => new JoinResult<TSource, TInner>() { Inner = a, Outer = b });
            return new ReadOnlyQueryable<JoinResult<TSource, TInner>>(dbcontext, newSource);
        }
        public new IReadOnlyQueryable<TResult> Join<TInner, TKey, TResult>(Expression<Func<TSource, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TSource, TInner, TResult>> resultSelector) where TInner : class
        {
            var newSource = source.Join(dbcontext.Set<TInner>(), outerKeySelector, innerKeySelector, resultSelector);
            return new ReadOnlyQueryable<TResult>(dbcontext, newSource);
        }
        public new IReadOnlyQueryable<TResult> GroupJoin<TInner, TKey, TResult>(Expression<Func<TSource, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TSource, IEnumerable<TInner>, TResult>> resultSelector) where TInner : class
        {
            var newSource = source.GroupJoin(dbcontext.Set<TInner>(), outerKeySelector, innerKeySelector, resultSelector);
            return new ReadOnlyQueryable<TResult>(dbcontext, newSource);
        }
        public new IReadOnlyQueryable<TResult> LeftJoin<TInner, TKey, TResult>(Expression<Func<TSource, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TSource, TInner, TResult>> resultSelector) where TInner : class
        {
#pragma warning disable EF1001 // Internal EF Core API usage.
            var newSource = source.LeftJoin(dbcontext.Set<TInner>(), outerKeySelector, innerKeySelector, resultSelector);
#pragma warning restore EF1001 // Internal EF Core API usage.
            return new ReadOnlyQueryable<TResult>(dbcontext, newSource);
        }
        public new IReadOnlyQueryable<IGrouping<TKey, TSource>> GroupBy<TKey>(Expression<Func<TSource, TKey>> keySelector)
        {
            var newSource = source.GroupBy(keySelector);
            return new ReadOnlyQueryable<IGrouping<TKey, TSource>>(dbcontext, newSource);
        }
        #endregion join

        #region Where
        public new DataProvider.IReadWriteQueryable<TSource> Where(Expression<Func<TSource, bool>> predicate)
        {
            source = source.Where(predicate);
            return this;
        }

        public new IReadWriteQueryable<TSource> WhereIf(bool condition, Expression<Func<TSource, bool>> expression)
        {
            if (condition)
            {
                return Where(expression);
            }
            return this;
        }

        public new IReadWriteQueryable<TSource> WhereIf(string condition, Expression<Func<TSource, bool>> expression)
        {
            return WhereIf(!string.IsNullOrEmpty(condition),expression);
        }

        public new IReadWriteQueryable<TSource> WhereIf(Guid? condition, Expression<Func<TSource, bool>> expression)
        {
            return WhereIf(condition != null && condition != Guid.Empty, expression);
        }
        #endregion Where

        #region Add
        public async Task<Guid> AddAsync(TSource data)
        {
            await dbcontext.AddAsync(data!);
            await dbcontext.SaveChangesAsync();
            return (data as IDataTable)!.Id;
            //throw new DebugException("新增数据失败，请检查业务逻辑");
        }

        public async Task<Guid> AddIfNotExistedAsync(Expression<Func<TSource,bool>> existedWhere,string message,Func<TSource> data)
        {
            if(await source.Where(existedWhere).AnyAsync())
            {
                throw new Nice.BuzinessException(message);
            }
            var item = data();
            return await AddAsync(item);
        }
        public Task AddAsync(TSource[] datas)
        {
            return DefaultBatchInsert(datas);
        }
        #endregion Add

        #region Delete
        public Task BulkDeleteAsync()
        {
            return source.ExecuteDeleteAsync();
        }
        public virtual async Task DeleteAsync()
        {
            var cnt = await DefaultBatchDelete();
            switch (cnt)
            {
                case 1:
                    break;
                case 0:
                    throw new DebugException("删除失败，未找到要删除的数据");
                default:
                    // 做不到,因为删除子数据的也会被统计进来
                    //throw new DebugException("删除数据超过1条，请检查业务逻辑.")
                    break;
            }
        }
        public async Task DeleteAsync(Guid id)
        {
            source = source.Where(o => (o as IDataTable)!.Id == id);
            await DeleteAsync();
        }
        public virtual async Task<int> DeleteAllAsync()
        {
            return await DefaultBatchDelete();
        }

        public async Task DeleteAsync(Guid[] ids)
        {
            source = source.Where(o => ids.Contains((o as IDataTable)!.Id));
            await DeleteAllAsync();
            //可能有级联删除 所以这个判断不成立
            //if (cnt != ids.Count())
            //{
            //    throw new DebugException("删除数据数量不正确");
            //}
        }
        #endregion Delete

        #region Update
        public Task BulkUpdateAsync(Action<TSource> updateExpression)
        {
            var o = updateExpression.GetInvocationList();
            return source.ExecuteUpdateAsync(o =>
                o.SetProperty(p=>p,null)
                );
        }
        public async Task UpdateAsync(Guid id,Action<TSource> updateExpression)
        {
            source = source.Where(o=> (o as IDataTable)!.Id == id);
            await UpdateAsync(updateExpression);
        }
        public async Task UpdateAsync(Action<TSource> updateExpression)
        {
            await DefaultBatchUpdate(updateExpression);
            //switch (cnt)
            //{
            //    case 1:
            //        break;
            //    case 0:
            //        //当指定的字段都未更新的时候，就会返回更新了0条数据，那么不能报错
            //        throw new DebugException("更新失败，未找到要更新的数据");
            //    default:
            //        break;
            //}
        }

        public async Task UpdateAsync(Guid[] ids, Action<TSource> updateExpression)
        {
            source = source.Where(o=> ids.Contains((o as IDataTable)!.Id));
            await UpdateAsync(updateExpression);
        }

        public TSource[] Source => source.ToArray();

        private async Task<int> DefaultBatchDelete()
        {
            var items = await source.Select(o=>o).ToArrayAsync();
            foreach (var item in items)
            {
                dbcontext.RemoveRange(item!);
            }
            return await dbcontext.SaveChangesAsync();
        }
        private async Task<int> DefaultBatchInsert(IEnumerable<TSource> items)
        {
            foreach (var item in items)
            {
                dbcontext.AddRange(item!);
            }
            return await dbcontext.SaveChangesAsync();
        }
        private async Task<int> DefaultBatchUpdate(Action<TSource> updateExpression)
        {
            var items = await source.ToArrayAsync();
            foreach (var item in items)
            {
                updateExpression(item);
            }
            return await dbcontext.SaveChangesAsync();
        }
        private async Task<int> DefaultBatchUpdate()
        {
            return await dbcontext.SaveChangesAsync();
        }
        #endregion Update

        #region 事务
        /// <summary>
        /// 使用数据库事务处理
        /// </summary>
        /// <param name="action"></param>
        public async Task Transaction(Func<Task> action)
        {
            using var tran = dbcontext.Database.BeginTransaction();
            try
            {
                await action();
                tran.Commit();
            }
            catch (TransactionException transEx)
            {
                tran.Rollback();
                throw transEx;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                throw ex;
            }
            finally
            {
                tran.Dispose();
            }
        }

        public Task<int> UpdateAsync()
        {
            return DefaultBatchUpdate();
        }
        #endregion 事务

        #region 排序
        public new IReadWriteQueryable<TSource> OrderBy<TProperty>(Expression<Func<TSource, TProperty>> predicate)
        {
            source = source.OrderBy(predicate);
            return this;
        }
        public new IReadWriteQueryable<TSource> OrderByDesc<TProperty>(Expression<Func<TSource, TProperty>> predicate)
        {
            source = source.OrderByDescending(predicate);
            return this;
        }
        #endregion 排序
    }
}