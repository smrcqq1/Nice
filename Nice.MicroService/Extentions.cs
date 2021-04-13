using Microsoft.Extensions.DependencyInjection;
using Nice.ORM;

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
        /// <summary>
        /// 使用自定义的RPC方案
        /// </summary>
        /// <returns></returns>
        public static IServiceCollection UseRPC<T>(this IServiceCollection Services) where T : class, IRPC
        {
            Services.AddSingleton<IRPC, T>();
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
