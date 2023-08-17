using Microsoft.Extensions.DependencyInjection;
using System;
namespace Nice.Tests
{
    /// <summary>
    /// 测试宿主
    /// </summary>
    public interface ITestHost
    {
        /// <summary>
        /// 
        /// </summary>
        IServiceCollection Services { get; }
        /// <summary>
        /// 获取指定的服务
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <returns></returns>
        /// <remarks>
        /// 将自己的服务注入，框架会自动解析依赖的其它服务，并生成mock数据
        /// </remarks>
        TService GetService<TService>();
        /// <summary>
        /// 增加一个服务
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TImplement"></typeparam>
        /// <returns></returns>
        /// <remarks>
        /// 如果是本地服务，直接获取mock数据
        /// 如果是远程服务，自动从指定源调用同名接口获取数据
        /// </remarks>
        ITestHost AddScoped<TService, TImplement>() where TService : class where TImplement : class, TService;
        /// <summary>
        /// 增加一个服务
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TImplement"></typeparam>
        /// <returns></returns>
        /// <remarks>
        /// 如果是本地服务，直接获取mock数据
        /// 如果是远程服务，自动从指定源调用同名接口获取数据
        /// </remarks>
        ITestHost AddSingleton<TService, TImplement>() where TService : class where TImplement : class, TService;

        /// <summary>
        /// 增加一个服务
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TImplement"></typeparam>
        /// <returns></returns>
        /// <remarks>
        /// 如果是本地服务，直接获取mock数据
        /// 如果是远程服务，自动从指定源调用同名接口获取数据
        /// </remarks>
        ITestHost AddTransient<TService, TImplement>() where TService : class where TImplement : class, TService;
        /// <summary>
        /// 增加一个服务
        /// </summary>
        ITestHost AddTransient(Type type1, Type type2);
    }
}