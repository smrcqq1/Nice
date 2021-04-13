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
            source = GetSource<TSource>(source);
            var res = new ReadWriteQueryable<TSource>(_dbContext, source);
            return res;
        }

        public IReadOnlyQueryable<TSource> ReadOnlySet<TSource>() where TSource : class, IEntitybase
        {
            var source = _dbContext.Set<TSource>().AsNoTracking();
            source = GetSource<TSource>(source);
            return new ReadOnlyQueryable<TSource>(_dbContext, source);
        }

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