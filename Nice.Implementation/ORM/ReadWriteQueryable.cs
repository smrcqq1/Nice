#region using
using Microsoft.EntityFrameworkCore;
using Nice.Core;
using Nice.DTO;
using Nice.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Transactions;
#endregion using
namespace Nice.ORM.EFCore
{
    /// <summary>
    /// 可读可写的数据库查询封装类
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    public class ReadWriteQueryable<TSource> : ReadOnlyQueryable<TSource>,IReadWriteQueryable<TSource> where TSource:class,IEntitybase
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

        #region Where
        public new IReadWriteQueryable<TSource> Where(Expression<Func<TSource, bool>> predicate)
        {
            source = source.Where(predicate);
            return this;
        }
        #endregion Where

        #region Add
        public async Task<bool> AddAsync(TSource data)
        {
            await dbcontext.AddAsync(data);
            await dbcontext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddAsync(IEnumerable<TSource> datas)
        {
            await dbcontext.BulkInsertAsync(datas);
            return true;
        }
        #endregion Add

        #region Delete
        protected async Task<bool> DeleteAndCheckAsync(int cnt)
        {
            var res = await source.DeleteFromQueryAsync();
            return res == cnt;
        }
        public virtual async Task<bool> DeleteAsync()
        {
            await source.DeleteFromQueryAsync();
            return true;
        }
        public Task<bool> DeleteAsync(int id)
        {
            source =  source.Where(o=>o.ID == id);
            return DeleteAndCheckAsync(1);
        }
        public Task<bool> DeleteAsync(IEnumerable<int> ids)
        {
            source = source.Where(o => ids.Contains(o.ID));
            return DeleteAndCheckAsync(ids.Count());
        }

        public Task<bool> DeleteAsync(TSource data)
        {
            if (data.ID < 1)
            {
                NiceException.Throw($"待删除数据ID不正确{data.ID}");
            }
            return DeleteAsync(data.ID) ;
        }
        public virtual async Task<bool> DeleteAsync(IEnumerable<TSource> datas)
        {
            await dbcontext.BulkDeleteAsync(datas);
            return true;
        }
        #endregion Delete

        #region Update
        public async Task<bool> UpdateAsync(Expression<Func<TSource, TSource>> updateExpression)
        {
            await source.UpdateFromQueryAsync(updateExpression);
            return true;
        }
        public Task<bool> UpdateAsync(int id,Expression<Func<TSource, TSource>> updateExpression)
        {
            source = source.Where(o => o.ID == id);
            return UpdateAsync(updateExpression);
        }
        #endregion Update


        #region 事务
        /// <summary>
        /// 使用数据库事务处理
        /// </summary>
        /// <param name="action"></param>
        public async Task Transaction(Func<IReadWriteQueryable<TSource>, Task> action)
        {
            using var tran = dbcontext.Database.BeginTransaction();
            try
            {
                await action(this);
                tran.Commit();
            }
            catch (TransactionException transEx)
            {
                tran.Rollback();
                throw transEx;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                tran.Dispose();
            }
        }
        #endregion 事务
    }
}