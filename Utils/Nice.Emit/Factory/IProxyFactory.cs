#region using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

#endregion using
namespace Nice.Emit
{
    /// <summary>
    /// 代理构建器
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    public interface IProxyFactory
    {
        /// <summary>
        /// 获取代理使用的Assembly，一般用于调试
        /// </summary>
        Assembly Assembly { get; }
        /// <summary>
        /// 当生成类代理的时候，需要对类进行的额外处理;目前仅需要对Attribute进行额外处理
        /// </summary>
        Action<TypeBuilder,Type> OnCreateType { get; set; }
        /// <summary>
        /// 当生成方法代理的时候，需要对方法进行的额外处理;目前仅需要对Attribute进行额外处理
        /// </summary>
        Func<MethodBuilder, MethodInfo,Action<ParameterBuilder, MethodInfo, ParameterInfo>> OnCreateMethod { get; set; }
        ///// <summary>
        ///// 当生成参数的时候，需要对参数进行的额外处理;目前仅需要对Attribute进行额外处理
        ///// </summary>
        //Action<ParameterBuilder, MethodInfo, ParameterInfo> OnCreateParameter { get; set; }
        /// <summary>
        /// 在代理的assmly中增加一个依赖注入类
        /// </summary>
        /// <param name="extendType">要继承的基类的类型</param>
        /// <returns></returns>
        /// <remarks>
        /// 1.代理方法实际调用的方法通过构造函数注入
        /// 2.需要指定生命周期
        /// </remarks>
        IInjectTypeBuilder CreateInjectType<TInterface>(Type? extendType = null);
        /// <summary>
        /// 在代理的assmly中增加一个DllImport类
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <param name="dllPath">通过DllImport引入的类库的路径</param>
        /// <returns></returns>
        /// <remarks>代理方法实际调用的方法通过DllImport引入</remarks>

        IDllImportProxyBuilder CreateDllImportType<TInterface>(string dllPath);
        /// <summary>
        /// 批量生成代理类
        /// </summary>
        /// <param name="types"></param>
        /// <param name="extendType"></param>
        /// <returns></returns>
        Assembly CreateInjectTypes(IEnumerable<Type> types, Type? extendType = null);
    }
}