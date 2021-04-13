using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;
using Nice.ORM;
using Nice.RPC;
using System;
using System.Linq;
using System.Reflection;

namespace Nice
{
    public static class Extentions
    {
        /// <summary>
        /// 使用微服务的基本框架
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection UseMicroService(this IServiceCollection services)
        {
            return services;
        }
        private static readonly ProxyGenerator _proxyGenerator = new ProxyGenerator();
        /// <summary>
        /// 使用自定义的RPC方案
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// 将其它微服务的Service注入进来,然后可以像本地Service一样使用
        /// </remarks>
        public static IServiceCollection UseRPC<TRPC>(this IServiceCollection Services, Assembly assembly) where TRPC : IRPC, IInterceptor
        {
            var list = assembly.GetTypes().Where(o=>o.IsInterface && o.IsPublic);
            var rpc = Activator.CreateInstance<TRPC>();
            //var httpRequestInterceptor = new HttpRequestInterceptor(rpc);
            foreach (var item in list)
            {
                var res = _proxyGenerator.CreateInterfaceProxyWithoutTarget(item, rpc);
                Services.AddSingleton(item, res);
            }
            return Services;
        }
        /// <summary>
        /// 使用自定义的消息队列
        /// </summary>
        /// <returns></returns>
        public static IServiceCollection UseMessageQueue<T>(this IServiceCollection Services) where T : class, IMessageQueue
        {
            Services.AddSingleton<IMessageQueue, T>();
            return Services;
        }
        /// <summary>
        /// 使用自定义的读写分离方案
        /// </summary>
        /// <returns></returns>
        public static IServiceCollection UseReadWriteSeparation<TSetting>(this IServiceCollection Services) where TSetting : class, IReadWriteSeparationSettings
        {
            Services.AddSingleton<IReadWriteSeparationSettings, TSetting>();
            return Services;
        }
    }
}
