using Microsoft.Extensions.DependencyInjection;
using Nice.WebAPI;
using System;
using System.Linq.Expressions;

namespace Nice.Caching;
/// <summary>
/// 缓存设置
/// </summary>
/// <remarks>
/// 应该包括以下几种方案，并且可以轻松的在各方案间自由切换
/// 1.可以在接口上打上缓存标记，并配置过期时间，框架自动采用netcore原生缓存方案
/// 2.可以为某个接口指定更新缓存标记如某aggr()接口，在标记中指定更新缓存的接口如update()，当update接口被调用的时候，自动更新aggr的缓存数据,并且可以指定缓存的实现如redis、memorycache、静态化等
/// 3.可以对数据库模型Entity进行配置，对这个模型的访问（最好可以支持部分字段来自缓存部分字段来自数据库）将自动调用指定的缓存
/// 
/// PS:缓存设置不在接口上以Attribute方式指定，是因为缓存跟业务逻辑和前端都无关，放在接口上标注会干扰业务逻辑和接口的讨论以及如果缓存配置改变就会导致Contracts的更新,Contracts原则上只有业务逻辑变更才需要更新
/// </remarks>
public static class Extentions
{
    /// <summary>
    /// 配置自定义缓存方案，并通过Nice框架自动管理
    /// </summary>
    /// <param name="provider"></param>
    /// <remarks>允许添加多个、多种类型的缓存，并且如果对同一接口进行了多个缓存配置，则会自动采用多级缓存</remarks>
    public static ICachingBuilder AddCaching(this INiceControllerBuilder builder, ICachingProvider cachingProviders)
    {
        var res = new CachingBuilder();
        res.AddCaching(cachingProviders);
        builder.AddCaching(res);
        return res;
    }
    /// <summary>
    /// 使用默认的内存缓存方案，并通过Nice框架自动管理
    /// </summary>
    /// <param name="provider"></param>
    /// <remarks>允许添加多个、多种类型的缓存，并且如果对同一接口进行了多个缓存配置，则会自动采用多级缓存</remarks>
    public static ICachingBuilder AddMemoryCaching(this ICachingBuilder provider)
    {
        return provider.AddCaching(new MemoryCachingProvider());
    }
    /// <summary>
    /// 使用默认的内存缓存方案，并通过Nice框架自动管理
    /// </summary>
    /// <param name="provider"></param>
    /// <remarks>允许添加多个、多种类型的缓存，并且如果对同一接口进行了多个缓存配置，则会自动采用多级缓存</remarks>
    public static ICachingBuilder AddResponseCaching(this INiceControllerBuilder builder)
    {
        return builder.AddCaching(new ResponseCachingProvider());
    }
    /// <summary>
    /// 使用默认的内存缓存方案，并通过Nice框架自动管理
    /// </summary>
    /// <param name="provider"></param>
    /// <remarks>允许添加多个、多种类型的缓存，并且如果对同一接口进行了多个缓存配置，则会自动采用多级缓存</remarks>
    public static ICachingBuilder AddMemoryCaching(this INiceControllerBuilder provider)
    {
        return provider.AddCaching(new MemoryCachingProvider());
    }
    /// <summary>
    /// 使用默认的内存缓存方案，并通过Nice框架自动管理
    /// </summary>
    /// <param name="provider"></param>
    /// <remarks>允许添加多个、多种类型的缓存，并且如果对同一接口进行了多个缓存配置，则会自动采用多级缓存</remarks>
    public static ICachingBuilder AddResponseCaching(this ICachingBuilder builder)
    {
        return builder.AddCaching(new ResponseCachingProvider());
    }
    /// <summary>
    /// 配置数据库数据的缓存，使用默认的内存缓存方案，并通过Nice框架自动管理
    /// </summary>
    /// <param name="provider"></param>
    /// <remarks>允许添加多个、多种类型的缓存，并且如果对同一接口进行了多个缓存配置，则会自动采用多级缓存</remarks>
    public static ICachingBuilder AddCaching<TEntity>(this INiceControllerBuilder provider) where TEntity : IDataTable
    {
        return provider.AddCaching(new MemoryCachingProvider());
    }
    /// <summary>
    /// 通过IServiceCollection配置缓存方案，这种方案需要开发手动控制
    /// </summary>
    /// <param name="provider"></param>
    public static IServiceCollection UseCaching<TCacheProvider>(this IServiceCollection services) where TCacheProvider : class, ICachingProvider
    {
        services.AddSingleton<ICachingProvider, TCacheProvider>();
        return services;
    }
    /// <summary>
    /// 通过IServiceCollection配置缓存方案，这种方案需要开发手动控制
    /// </summary>
    /// <param name="provider"></param>
    public static IServiceCollection UseCaching(this IServiceCollection services)
    {
        services.AddSingleton<ICachingProvider, MemoryCachingProvider>();
        return services;
    }
    /// <summary>
    /// 设置更新此缓存的方法，当此方法被调用，在Add方法中指定的接口将被调用，于是缓存将自动被更新
    /// </summary>
    /// <returns></returns>
    public static ICachingBuilder When<TInterface, TRequest>(this ICachingBuilder builder, Expression<Func<TInterface, Func<TRequest, Task>>> updateActionSelector, Expression<Func<TRequest,Guid>>? keySelector = null)
    {
        builder.Current.UpdateExpression.Add(updateActionSelector);
        return builder;
    }
    /// <summary>
    /// 设置更新此缓存的方法，当此方法被调用，在Add方法中指定的接口将被调用，于是缓存将自动被更新
    /// </summary>
    /// <returns></returns>
    public static ICachingBuilder When<TInterface>(this ICachingBuilder builder, Expression<Func<TInterface, MulticastDelegate>> updateActionSelector)
    {
        builder.Current.UpdateExpression.Add(updateActionSelector);
        return builder;
    }
    /// <summary>
    /// 设置更新此缓存的方法，当此方法被调用，在Add方法中指定的接口将被调用，于是缓存将自动被更新
    /// </summary>
    /// <returns></returns>
    public static ICachingBuilder When<TInterface, TRequest,TResult>(this ICachingBuilder builder, Expression<Func<TInterface, Func<TRequest, Task<TResult>>>> updateActionSelector, Func<TRequest, TResult,Guid>? keySelector = null)
    {
        builder.Current.UpdateExpression.Add(updateActionSelector);
        return builder;
    }
    /// <summary>
    /// 设置更新此缓存的方法:当此方法被调用，在Set方法中指定的接口将被调用，于是缓存将自动被更新
    /// </summary>
    /// <returns></returns>
    public static ICachingBuilder<TInterface> When<TInterface>(this ICachingBuilder<TInterface> builder, Expression<Func<TInterface, MulticastDelegate>> updateActionSelector)
    {
        builder.Current.UpdateExpression.Add(updateActionSelector);
        return builder;
    }
    /// <summary>
    /// 设置更新此缓存的方法:过期时间，单位：秒
    /// </summary>
    /// <returns></returns>
    public static ICachingBuilder Expire(this ICachingBuilder builder, int second)
    {
        builder.Current.ExpireSecond = second;
        return builder;
    }
    /// <summary>
    /// 设置更新此缓存的方法:滑动过期时间，单位：秒
    /// </summary>
    /// <returns></returns>
    public static ICachingBuilder SlidingExpire(this ICachingBuilder builder)
    {
        return builder;
    }
    
    /// <summary>
    /// 设置指定接口的数据要被自动缓存
    /// </summary>
    /// <typeparam name="TInterface">两个接口来自同一个class</typeparam>
    /// <param name="actionSelector">指定要缓存的接口</param>
    /// <param name="updateActionSelector">指定更新缓存的接口</param>
    /// <returns></returns>
    /// <remarks>
    /// 1.缓存key生成方案：自动采用updateActionSelector的参数中的id字段的值作为缓存key，若不存在，则报错
    /// </remarks>
    public static ICachingBuilder Set<TInterface>(this ICachingBuilder builder,
        Expression<Func<TInterface, MulticastDelegate>> actionSelector,
        Expression<Func<TInterface, MulticastDelegate>> updateActionSelector
        )
    {
        return builder.Set<TInterface, TInterface>(actionSelector, updateActionSelector);
    }
    /// <summary>
    /// 设置指定接口的数据要被自动缓存，支持两个接口不在同一个class
    /// </summary>
    /// <typeparam name="TCachingInterface">缓存接口所在class</typeparam>
    /// <typeparam name="TUpdateInterface">更新缓存接口所在class</typeparam>
    /// <param name="actionSelector">指定要缓存的接口</param>
    /// <param name="updateActionSelector">指定更新缓存的接口</param>
    /// <returns></returns>
    /// <remarks>
    /// 1.缓存key生成方案：自动采用updateActionSelector的参数中的id字段的值作为缓存key，若不存在，则报错
    /// </remarks>
    public static ICachingBuilder Set<TCachingInterface,TUpdateInterface>(this ICachingBuilder builder,
        Expression<Func<TCachingInterface, MulticastDelegate>> actionSelector,
        Expression<Func<TUpdateInterface, MulticastDelegate>> updateActionSelector
        )
    {
        var setting = new CachingSetting();
        setting.ActionExpression.Add(actionSelector);
        setting.UpdateExpression.Add(updateActionSelector);
        builder.AddSetting(setting);
        return builder;
    }
    /// <summary>
    /// 设置指定接口的数据要被自动缓存
    /// </summary>
    /// <typeparam name="TInterface"></typeparam>
    /// <param name="actionSelector"></param>
    /// <returns></returns>
    public static ICachingBuilder<TInterface> Set<TInterface>(this ICachingBuilder builder, Expression<Func<TInterface, MulticastDelegate>> actionSelector,int? second = null)
    {
        var setting = new CachingSetting()
        {
            ExpireSecond = second
        };
        setting.ActionExpression.Add(actionSelector);
        builder.AddSetting(setting);
        var b = new CachingBuilder<TInterface>
        {
            Settings = builder.Settings,
            Current = builder.Current,
            CachingProviders = builder.CachingProviders
        };
        return b;
    }

    public static ICachingBuilder AddSetting(this ICachingBuilder builder, CachingSetting setting)
    {
        builder.Settings.Add(setting);
        builder.Current = setting;
        return builder;
    }
    /// <summary>
    /// 增加一个Provider，构成多级缓存
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="provider"></param>
    /// <returns></returns>
    public static ICachingBuilder AddCaching(this ICachingBuilder builder,ICachingProvider provider)
    {
        builder.CachingProviders.Add(provider);
        return builder;
    }
}
/// <summary>
/// 缓存配置器
/// </summary>
/// 大部分情况下，缓存都是统一配置的，所以应该走这个入口
internal class CachingBuilder : ICachingBuilder
{
    public List<ICachingProvider> CachingProviders { get; internal set; } = new List<ICachingProvider>();

    public List<CachingSetting> Settings { get; internal set; } = new List<CachingSetting>();

    public CachingSetting Current { get; set; }
}
/// <summary>
/// 
/// </summary>
/// <typeparam name="TInterface"></typeparam>
public interface ICachingBuilder<TInterface> : ICachingBuilder
{
    //todo 这个用来解决配置缓存的时候，调用When的时候老是要输入类型的问题，但是似乎实用性不高,这样当Update接口就在源接口一个类的时候可以不用反复When<T>
}
/// <summary>
/// 缓存配置器
/// </summary>
/// 大部分情况下，缓存都是统一配置的，所以应该走这个入口
internal class CachingBuilder<TInterface> : CachingBuilder, ICachingBuilder<TInterface>
{
}