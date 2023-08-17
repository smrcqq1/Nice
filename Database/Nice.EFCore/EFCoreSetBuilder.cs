#region using
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
#endregion using
namespace Nice.EFCore
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    public class EFCoreSetBuilder<TDbContext> where TDbContext : DbContext
    {
        public EFCoreSetBuilder(IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction)
        {
            services.AddScoped<IDataProvider, EFDataProvider<TDbContext>>();
            services.AddDbContextPool<TDbContext>(optionsAction);
        }

        public bool EnsureDeleted { get; set; }
        public bool EnsureCreated { get; set; }
    }
    /// <summary>
    /// 使用自动生成表的DbContext
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    public class AutoEFCoreSetBuilder<TAutoDbContext> : EFCoreSetBuilder<AutoDbContext> where TAutoDbContext : AutoDbContext
    {
        public AutoEFCoreSetBuilder(IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction) : base(services,optionsAction)
        {
        }
        /// <summary>
        /// 设置自动生成数据库表
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        public AutoEFCoreSetBuilder<TAutoDbContext> SetTables(params Type[] types)
        {
            AutoDbContext.Tables.AddRange(types.Where(o => !o.IsAbstract && typeof(IDataTable).IsAssignableFrom(o)));
            return this;
        }
        /// <summary>
        /// 设置自动生成数据库表
        /// </summary>
        /// <param name="assemblies"></param>
        /// <returns></returns>
        public AutoEFCoreSetBuilder<TAutoDbContext> SetTables(params Assembly[] assemblies)
        {
            var types = assemblies.SelectMany(x => x.GetTypes()).ToArray();
            return SetTables(types);
        }
    }
}