#region using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#endregion using
namespace Nice.Emit
{
    /// <summary>
    /// 生成一个代理，其代理的方法由构造函数注入的类中获取
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    public interface IInjectTypeBuilder: ITypeProxyBuilder
    {
        ///// 设置代理类的基类
        ///// </summary>
        ///// <param name="extendType"></param>
        ///// <returns></returns>
        //IInjectProxyBuilder SetExtendType(Type? extendType);
        ///// <summary>
        ///// 设置代理类的基类
        ///// </summary>
        ///// <param name="extendType"></param>
        ///// <returns></returns>
        //IInjectProxyBuilder SetExtendType<T>() where T : class;
        ///// <summary>
        ///// 添加一个构造函数
        ///// </summary>
        ///// <param name="constructorArguments">构造函数参数</param>
        ///// <returns></returns>
        //IInjectProxyBuilder AddConstructor(params Type[] constructorArguments);
    }
}