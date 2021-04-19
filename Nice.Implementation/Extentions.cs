#region using
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Nice;
using Nice.Cache;
using Nice.Logger;
using Nice.ORM;
using Nice.ORM.EFCore;
#endregion using

namespace System
{
    /// <summary>
    /// 统一扩展
    /// </summary>
    public static class Extentions
    {
        /// <summary>
        /// 在项目中使用正常的封装,可以满足大部分需求
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        /// <remarks>
        /// 1.默认缓存方案使用Redis和静态化两种
        /// 2.默认RPC方案使用Http
        /// 3.默认ORM框架使用EFCore
        /// 4.默认日志框架使用NLog
        /// PS:引用以后也可以自行替换部分自定义实现
        /// </remarks>
        public static IServiceCollection UseNice(this IServiceCollection services)
        {
            services.UseCache<Redis>()
                .UseLogger<NLog>();
            return services;
        }
        /// <summary>
        /// 使用自定义的缓存方案
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// 1.如果只使用一种缓存请使用此方法注入,后注入的会覆盖前面的;业务代码统一注入ICache即可
        /// 2.如果同时使用几种缓存方案,需要调用别的方法注入,例如调用UseCache()使用默认的Redis缓存,再调用UseStaticize()使用默认的静态化缓存.这样,业务代码中要使用redis,则注入ICache,要使用静态化则注入IStaticize,即可同时使用多种缓存
        /// </remarks>
        public static IServiceCollection UseCache<T>(this IServiceCollection services) where T : class, ICache
        {
            services.AddSingleton<ICache, T>();
            return services;
        }
        /// <summary>
        /// 使用redis缓存方案
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection UseRedis(this IServiceCollection services)
        {
            services.AddSingleton<ICache, Redis>();
            return services;
        }
        /// <summary>
        /// 使用自定义的ORM框架
        /// </summary>
        /// <returns></returns>
        public static IServiceCollection UseORM<T, TDbContext>(this IServiceCollection services) where T : class, IORM where TDbContext : DbContext
        {
            services.AddScoped<IORM, T>();
            return services;
        }
        /// <summary>
        /// 使用EFCore
        /// </summary>
        /// <returns></returns>
        public static IServiceCollection UseEFCore<TDbContext>(this IServiceCollection services,Action<DbContextOptionsBuilder> optionsAction) where TDbContext : DbContext
        {
            services.AddDbContextPool<TDbContext>(optionsAction);
            return services.UseORM<EFCore<TDbContext>, TDbContext>();
        }
        /// <summary>
        /// 使用自定义的日志方案
        /// </summary>
        /// <returns></returns>
        public static IServiceCollection UseLogger<T>(this IServiceCollection services) where T : class, ILogger
        {
            services.AddSingleton<ILogger, T>();
            return services;
        }
        /// <summary>
        /// 使用多租户的用户信息
        /// </summary>
        /// <returns></returns>
        public static IServiceCollection UseTenated<T>(this IServiceCollection services) where T : class, ITenantProvider
        {
            services.AddScoped<ITenantProvider, T>();
            return services;
        }
        /// <summary>
        /// 使用读写分离
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="services"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static IServiceCollection UseReadWriteSeparation<T>(this IServiceCollection services,IReadWriteSeparationSettings settings) where T :class, IReadWriteSeparation
        {
            return services;
        }
    }
}