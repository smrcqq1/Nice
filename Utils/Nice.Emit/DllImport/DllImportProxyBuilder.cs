#region using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

#endregion using
namespace Nice.Emit
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    internal class DllImportProxyBuilder : NiceTypeBuilder, IDllImportProxyBuilder
    {
        public DllImportProxyBuilder(ProxyFactory proxyBuilder, string dllPath, Type interfaceType) : base(proxyBuilder, interfaceType,null)
        {
            this.dllPath = dllPath;
        }

        private readonly string dllPath;

        #region 创建指定类型的DllImport代理类
        /// <summary>
        /// 创建指定类型的DllImport代理类
        /// </summary>
        /// <returns>代理类</returns>
        public override Type Create()
        {
            var methods = _interfaceType.GetMethods();

            var nativeType = CreateNative(dllPath,methods);

            _typeBuilder = CreateDynamicProxy(_proxyBuilder._moduleBuilder, _interfaceType, nativeType, methods);

            _typeBuilder.AddInterfaceImplementation(_interfaceType);

            var type = _typeBuilder.CreateType();
            return type;
        }
        #endregion 创建指定类型的DllImport代理类

        #region 创建一个DllImport的静态代理类
        /// <summary>
        /// 创建一个DllImport的静态代理类
        /// </summary>
        private Type CreateNative(string dllPath,IEnumerable<MethodInfo> methods)
        {
            foreach (var method in methods)
            {
                Type[] parameterTypes = GetParameterTypes(method.GetParameters());

                var builder = _typeBuilder.DefinePInvokeMethod(method.Name
                    , dllPath
                    , MethodAttributes.Public | MethodAttributes.Static | MethodAttributes.PinvokeImpl
                    , CallingConventions.Standard
                    , method.ReturnType
                    , parameterTypes
                    , CallingConvention.Cdecl
                    , CharSet.Ansi);
                builder.SetImplementationFlags(builder.GetMethodImplementationFlags() | MethodImplAttributes.PreserveSig);
            }
            var resultType = _typeBuilder.CreateType();
            if (null == resultType)
            {
                throw new InvalidOperationException("生成DllImport静态代理类失败");
            }
            return resultType;
        }
        #endregion 创建一个DllImport的静态代理类
    }
}
