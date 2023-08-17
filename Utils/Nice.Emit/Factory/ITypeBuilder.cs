using System;
namespace Nice.Emit
{
    /// <summary>
    /// 代理类创建器
    /// </summary>
    public interface  ITypeProxyBuilder
    {
        /// <summary>
        /// 添加一个构造函数
        /// </summary>
        /// <param name="constructorArguments">构造函数参数</param>
        /// <returns></returns>
         ITypeProxyBuilder AddConstructor(params Type[] constructorArguments);
        /// <summary>
        /// 根据配置创建类型
        /// </summary>
        /// <returns></returns>
        Type Create();
    }
}