using Microsoft.Extensions.DependencyInjection;
using Nice.Cache;
using System;

namespace Nice
{
    /// <summary>
    /// 扩展封装
    /// </summary>
    public static class Extentions
    {
        /// <summary>
        /// 使用默认的静态化缓存
        /// </summary>
        /// <returns></returns>
        public static IServiceCollection UseStaticize(this IServiceCollection Services)
        {
            Services.AddSingleton<IStaticize, Staticize>();
            return Services;
        }
        /// <summary>
        /// 使用自定义的静态化缓存
        /// </summary>
        /// <returns></returns>
        public static IServiceCollection UseStaticize<T>(this IServiceCollection Services) where T : class, IStaticize
        {
            Services.AddSingleton<IStaticize, T>();
            return Services;
        }
    }
}