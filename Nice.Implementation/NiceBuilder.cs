//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.DependencyInjection;
//using Nice.Core;
//using Nice.ORM;
//using Nice.Cache;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Nice.ORM.EFCore;

//namespace Nice
//{
//    /// <summary>
//    /// 项目配置器
//    /// </summary>
//    public class NiceBuilder
//    {
//        public NiceBuilder(IServiceCollection services)
//        {
//            Services = services;
//        }
//        internal IServiceCollection Services { get; }
//        /// <summary>
//        /// 使用自定义的缓存方案
//        /// </summary>
//        /// <returns></returns>
//        /// <remarks>
//        /// 1.如果只使用一种缓存请使用此方法注入,后注入的会覆盖前面的;业务代码统一注入ICache即可
//        /// 2.如果同时使用几种缓存方案,需要调用别的方法注入,例如调用UseCache()使用默认的Redis缓存,再调用UseStaticize()使用默认的静态化缓存.这样,业务代码中要使用redis,则注入ICache,要使用静态化则注入IStaticize,即可同时使用多种缓存
//        /// </remarks>
//        public NiceBuilder UseCache<T>() where T : class, ICache
//        {
//            Services.AddSingleton<ICache, T>();
//            return this;
//        }
//        /// <summary>
//        /// 使用自定义的RPC方案
//        /// </summary>
//        /// <returns></returns>
//        public NiceBuilder UseRPC<T>() where T : class, IRPC
//        {
//            Services.AddSingleton<IRPC, T>();
//            return this;
//        }
//        /// <summary>
//        /// 使用默认的静态化缓存
//        /// </summary>
//        /// <returns></returns>
//        public NiceBuilder UseStaticize()
//        {
//            Services.AddSingleton<IStaticize, Staticize>();
//            return this;
//        }
//        /// <summary>
//        /// 使用自定义的静态化缓存
//        /// </summary>
//        /// <returns></returns>
//        public NiceBuilder UseStaticize<T>() where T : class, IStaticize
//        {
//            Services.AddSingleton<IStaticize, T>();
//            return this;
//        }
//        /// <summary>
//        /// 使用自定义的ORM框架
//        /// </summary>
//        /// <returns></returns>
//        public NiceBuilder UseORM<T, TDbContext>() where T : class, IORM where TDbContext :DbContext
//        {
//            Services.AddScoped<IORM, T>();
//            return this;
//        }
//        /// <summary>
//        /// 使用EFCore
//        /// </summary>
//        /// <returns></returns>
//        public NiceBuilder UseEFCore<TDbContext>(Action<DbContextOptionsBuilder> optionsAction) where TDbContext : DbContext
//        {
//            Services.AddDbContextPool<TDbContext>(optionsAction);
//            return UseORM<EFCore<TDbContext>, TDbContext>();
//        }
//        /// <summary>
//        /// 使用自定义的消息队列
//        /// </summary>
//        /// <returns></returns>
//        public NiceBuilder UseMessageQueue<T>() where T : class, IMessageQueue
//        {
//            Services.AddSingleton<IMessageQueue, T>();
//            return this;
//        }
//        /// <summary>
//        /// 使用自定义的读写分离方案
//        /// </summary>
//        /// <returns></returns>
//        public NiceBuilder UseReadWriteSeparation<TSetting>() where TSetting : class, IReadWriteSeparationSettings
//        {
//            Services.AddSingleton<IReadWriteSeparationSettings, TSetting>();
//            return this;
//        }
//        /// <summary>
//        /// 使用自定义的日志方案
//        /// </summary>
//        /// <returns></returns>
//        public NiceBuilder UseLogger<T>() where T : class, ILogger
//        {
//            Services.AddSingleton<ILogger, T>();
//            return this;
//        }
//        /// <summary>
//        /// 使用多租户的用户信息
//        /// </summary>
//        /// <returns></returns>
//        public NiceBuilder UseTenated<T>() where T : class, ITenatedUserinfo
//        {
//            Services.AddScoped<ITenatedUserinfo, T>();
//            return this;
//        }
//    }
//}