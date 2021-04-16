using Microsoft.EntityFrameworkCore;
using Nice.Entities;
using System.Linq;

namespace Nice.ORM.EFCore
{
    /// <summary>
    /// EFCore版的ORM框架
    /// </summary>
    public class EFCore<TDbContext> : IORM where TDbContext :DbContext
    {
        private readonly TDbContext _dbContext;
        public EFCore(TDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IReadWriteQueryable<TSource> Set<TSource>() where TSource : class,IEntitybase
        {
            var source = _dbContext.Set<TSource>().AsQueryable();
            source = GetSource(source);
            var res = new ReadWriteQueryable<TSource>(_dbContext, source);
            return res;
        }

        public IReadOnlyQueryable<TSource> ReadOnlySet<TSource>() where TSource : class, IEntitybase
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
        static IQueryable<TSource> GetSource<TSource>(IQueryable<TSource> source) where TSource : class, IEntitybase
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
    }
}