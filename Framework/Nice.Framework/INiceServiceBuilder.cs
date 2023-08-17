using Microsoft.Extensions.DependencyInjection;

namespace Nice
{
    /// <summary>
    /// 
    /// </summary>
    public interface INiceServiceBuilder
    {
        /// <summary>
        /// 
        /// </summary>
        IServiceCollection Services
        {
            get;
        }
        /// <summary>
        /// 从两个Assembly中自动检索需要注册的服务依赖
        /// </summary>
        /// <typeparam name="TContracts">契约层的任意一个接口</typeparam>
        /// <typeparam name="TService">服务层的任意一个类</typeparam>
        /// <returns></returns>
        /// <remarks>
        /// 此方法含下列步骤：
        /// 1.自动查找TContracts所在Assembly中所有继承了Signs.ILifeCycle的接口
        /// 2.在TService所在Assembly查找所有实现了上一步找到的接口的实现
        /// 3.按照TContracts中的标记Signs.ILifeCycle(包含IScoped,ISingleton等常用三个生命周期)，决定注入的生命周期
        /// </remarks>
        INiceServiceBuilder AddServices<TContracts, TService>(Type typeofInterface) where TService : class, TContracts;

        /// <summary>
        /// 注入标记了Nice.Signs.IAPI的接口和实现
        /// </summary>
        /// <typeparam name="TContracts"></typeparam>
        /// <typeparam name="TService"></typeparam>
        /// <returns></returns>
        INiceServiceBuilder AddServices<TContracts, TService>()where TService : class, TContracts;
    }
}
