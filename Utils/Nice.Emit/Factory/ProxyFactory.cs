#region using
using System.Reflection;
using System.Reflection.Emit;
using System;
using System.Linq;
using System.Collections.Generic;
#endregion using
namespace Nice.Emit
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    public class ProxyFactory : IProxyFactory
    {
        #region 构造函数
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
        public ProxyFactory(string name)
#pragma warning restore CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
        {
            //定义程序集的名称
            var assemblyName = new AssemblyName(name);
            // 创建一个程序集构建器
            _assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.RunAndCollect);
            // 使用程序集构建器创建一个模块构建器
            _moduleBuilder = _assemblyBuilder.DefineDynamicModule(assemblyName.Name + ".dll");
        }
        #endregion 构造函数

        #region 字段
        /// <summary>
        /// 
        /// </summary>
        public readonly AssemblyBuilder _assemblyBuilder;
        /// <summary>
        /// 
        /// </summary>
        public readonly ModuleBuilder _moduleBuilder;
        #endregion 字段
        /// <summary>
        /// 
        /// </summary>
        public Assembly Assembly => _assemblyBuilder;
        /// <summary>
        /// 创建类型的时候需要执行的设置
        /// </summary>
        public Action<TypeBuilder, Type> OnCreateType { get; set; }
        /// <summary>
        /// 创建方法的时候需要执行的设置
        /// </summary>
        public Func<MethodBuilder, MethodInfo,Action<ParameterBuilder, MethodInfo, ParameterInfo>> OnCreateMethod { get; set; }

        /// <summary>
        /// 创建DllImport类型的代理
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <param name="dllPath"></param>
        /// <returns></returns>
        public IDllImportProxyBuilder CreateDllImportType<TInterface>(string dllPath)
        {
            var type = typeof(TInterface);
            return new DllImportProxyBuilder(this,dllPath, type);
        }
        /// <summary>
        /// 创建指定接口的代理
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <param name="extendType"></param>
        /// <returns></returns>
        public IInjectTypeBuilder CreateInjectType<TInterface>(Type? extendType = null)
        {
            var type = typeof(TInterface);
            return new InjectTypeBuilder(this, type, extendType);
        }
        /// <summary>
        /// 批量创建指定接口的代理
        /// </summary>
        /// <param name="types"></param>
        /// <param name="extendType"></param>
        /// <returns></returns>
        public Assembly CreateInjectTypes(IEnumerable<Type> types,Type? extendType = null)
        {
           var tmp =  types.Select(o=> new InjectTypeBuilder(this, o, extendType).Create()).ToArray();
            return this.Assembly;
        }
    }
}
