#region using
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Nice.EFCore;
#endregion using
namespace Nice
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    public static class EFCoreExtentions
    {
        public static AutoEFCoreSetBuilder<TAutoDbContext> UseAutoEFCore<TAutoDbContext>(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction) where TAutoDbContext : AutoDbContext
        {
            return new AutoEFCoreSetBuilder<TAutoDbContext>(services, optionsAction);
        }
        public static AutoEFCoreSetBuilder<AutoDbContext> UseAutoEFCore(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction)
        {
            return new AutoEFCoreSetBuilder<AutoDbContext>(services, optionsAction);
        }
        public static EFCoreSetBuilder<TDbContext> UseEFCore<TDbContext>(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction) where TDbContext : DbContext
        {
            return new EFCoreSetBuilder<TDbContext>(services, optionsAction);
        }
        /// <summary>
        /// 使用EFCore作为数据提供器
        /// </summary>
        /// <typeparam name="TDbContext"></typeparam>
        /// <param name="services"></param>
        /// <param name="optionsAction"></param>
        /// <returns></returns>
        //public static ISetBuilder UseEFCore(this ISetBuilder builder, Action<DbContextOptionsBuilder> optionsAction)
        //{
        //    AutoDbContext.Tables.AddRange(builder.Types);
        //    builder.ServiceCollection.AddScoped<EFDataProvider>();
        //    builder.ServiceCollection.AddDbContextPool<AutoDbContext>(optionsAction);
        //    return builder;
        //}
        ///// <summary>
        ///// 使用EFCore作为数据提供器
        ///// </summary>
        ///// <returns></returns>
        //public static DataProviderBuilder AddDataProviderByEFCore(this DataProviderBuilder builder
        //    , Action<DbContextOptionsBuilder> optionsAction
        //    , Assembly assembly
        //    , params Assembly[] assemblies)
        //{
        //    var types = assembly.GetTypes().Where(o => typeof(IDataTable).IsAssignableFrom(o)).ToList();
        //    if(assemblies!= null && assemblies.Length > 0)
        //    {
        //        foreach (var ass in assemblies)
        //        {
        //            types.AddRange(assembly.GetTypes().Where(o => typeof(IDataTable).IsAssignableFrom(o)).ToList());
        //        }
        //    }
        //    AutoDbContext.Tables = types.ToArray();
        //    builder.NiceBuilder.Services.UseEFCore<AutoDbContext>(optionsAction);
        //    return builder;
        //}
        ///// <summary>
        ///// 使用Redis作为数据提供器
        ///// </summary>
        ///// <returns></returns>
        ///// <remarks>
        ///// 这个要写到Nice.Redis包里面去
        ///// </remarks>
        //public static DataProviderBuilder WithRedis(this DataProviderBuilder builder,
        //    string url)
        //{
        //    //var types = assembly.GetTypes().Where(o => typeof(IDataTable).IsAssignableFrom(o)).ToArray();
        //    //AutoDbContext.Tables = types;
        //    //builder.Services.AddDbContextPool<AutoDbContext>(optionsAction);
        //    //builder.SetDataProvider<EFDataProvider<AutoDbContext>>();
        //    return builder;
        //}
    }
}

//namespace Nice.EFCore
//{
//    /// <summary>
//    /// 不使用Nice框架可以使用这个来注入EFCore
//    /// </summary>
//    public static class EFCoreExtentions
//    {
//        /// <summary>
//        /// 使用EFCore
//        /// </summary>
//        /// <returns></returns>
//        public static IServiceCollection UseEFCore<TDbContext>(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction) where TDbContext : DbContext
//        {
//            services.AddDbContextPool<TDbContext>(optionsAction);
//            return services.UseORM<EFDataProvider<TDbContext>, TDbContext>();
//        }
//        /// <summary>
//        /// 使用自定义的ORM框架
//        /// </summary>
//        /// <returns></returns>
//        public static IServiceCollection UseORM<T, TDbContext>(this IServiceCollection services) where T : class, IDataProvider where TDbContext : DbContext
//        {
//            services.AddScoped<IDataProvider, T>();
//            return services;
//        }
//    }
//}
