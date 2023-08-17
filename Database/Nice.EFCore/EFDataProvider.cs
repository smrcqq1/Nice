using Microsoft.EntityFrameworkCore;
using Nice.DataProvider;
using System.Linq.Expressions;

namespace Nice.EFCore
{
    internal class EFDataProvider : EFDataProvider<AutoDbContext>
    {
        public EFDataProvider(AutoDbContext autoDbContext):base(autoDbContext)
        {
        }
    }
    /// <summary>
    /// EFCore版的ORM框架
    /// </summary>
    public class EFDataProvider<TDbContext> : IEFCoreDataProvider<TDbContext> where TDbContext :DbContext
    {
        private readonly TDbContext _dbContext;
        public EFDataProvider(TDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        //public Func<IEnumerable<TSource>, Task<int>> BatchInsert;
        //public Func<Expression<Func<TSource, TSource>>, Task<int>> BatchUpdate<TSource>;

        public DataProvider.IReadWriteQueryable<TSource> Set<TSource>() where TSource : class, IDataTable,new()
        {
            var source = _dbContext.Set<TSource>().AsQueryable();
            source = GetSource(source);
            var res = new ReadWriteQueryable<TSource>(_dbContext, source);
            return res;
        }

        public IReadOnlyQueryable<TSource> ReadOnlySet<TSource>() where TSource : class, IDataTable, new()
        {
            var source = _dbContext.Set<TSource>().AsNoTracking();
            source = GetSource(source);
            return new ReadOnlyQueryable<TSource>(_dbContext, source);
        }
        /// <summary>
        /// 根据TSource类型获取不同的查询
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <remarks>
        /// 期望(尽量不装箱,不反射的前提下实现):
        /// 1.封装逻辑删除
        /// 2.封装多租户
        /// </remarks>
        static System.Linq.IQueryable<TSource> GetSource<TSource>(System.Linq.IQueryable<TSource> source) where TSource : class, new()
        {
            //if (typeof(ISoftDeletable).IsAssignableFrom(typeof(TSource)))
            //{
            //    source = source.WhereDynamic("IsDeleted = 0");
            //}
            //if (typeof(ITenated).IsAssignableFrom(typeof(TSource)))
            //{
            //    source = source.WhereDynamic("TeantID = 0");
            //}
            return source;
        }

        public Task AddAsync<T>(T newT) where T : class, IDataTable, new()
        {
             return Set<T>().AddAsync(newT);
        }

        public Task AddAsync<T>(T[] newT) where T : class,IDataTable, new()
        {
            return Set<T>().AddAsync(newT);
        }

        public Task DeleteAsync<T>(Guid id) where T : class, IDataTable, new()
        {
            return Set<T>().DeleteAsync(id);
        }

        public Task DeleteAsync<T>(Guid[] ids) where T : class, IDataTable, new()
        {
            return Set<T>().DeleteAsync(ids);
        }

        public Task UpdateAsync<T>(Guid id, Action<T> expression) where T : class, IDataTable, new()
        {
            return Set<T>().UpdateAsync(id, expression);
        }

        public Task UpdateAsync<T>(Guid[] ids, Action<T> expression) where T : class, IDataTable, new()
        {
            return Set<T>().UpdateAsync(ids, expression);
        }
    }
}