using Castle.DynamicProxy;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace Nice.RPC
{
    public class Proxy
    {
        private static readonly ProxyGenerator _proxyGenerator = new ProxyGenerator();
        //private static ConcurrentDictionary<string, object> _services = new ConcurrentDictionary<string, object>();


        //public static Type Get<T>(IRPC rPC) where T : class
        //{
        //    //Castle方式
        //    return _services.GetOrAdd(typeof(T).FullName, key =>
        //    {
        //        var httpRequestInterceptor = new HttpRequestInterceptor(rPC);
        //        return _proxyGenerator.CreateInterfaceProxyWithoutTarget<T>(httpRequestInterceptor);
        //    }).GetType();
        //}
        //public static object Get(Type type, IRPC rPC)
        //{
        //    var httpRequestInterceptor = new HttpRequestInterceptor(rPC);
        //    return _proxyGenerator.CreateInterfaceProxyWithoutTarget(type, httpRequestInterceptor);
        //}

        #region Emit方式
        public static T Get<T>() where T : class
        {
            return Create<T>();
        }
        public static Type Get(Type type)
        {
            return build(type);
        }
        public static T Create<T>()
        {
            // 从类型构建器中创建出类型
            var dynamicType = build<T>();
            // 通过反射创建出动态类型的实例
            var commander = Activator.CreateInstance(dynamicType);
            return (T)commander;
        }
        static Type build<T>()
        {
            var targetType = typeof(T);
            return build(targetType);
        }
        public static Type build(Type targetType)
        {
            //System.Reflection.AssemblyName 是用来表示一个Assembly的完整名称的
            var assyName = new AssemblyName();
            //为要创建的Assembly定义一个名称（这里忽略版本号，Culture等信息）
            assyName.Name = "MyAssyFor_" + targetType.Name;
            //获取AssemblyBuilder
            //AssemblyBuilderAccess有Run，Save，RunAndSave三个取值
            var assyBuilder =
               AssemblyBuilder.DefineDynamicAssembly(
                   assyName,
                   AssemblyBuilderAccess.Run);
            //获取ModuleBuilder，提供String参数作为Module名称，随便设一个
            var modBuilder = assyBuilder.DefineDynamicModule("MyModFor_" + targetType.Name);
            //真正创建，并返回
            return modBuilder.DefineType(targetType);
        }
        #endregion Emit方式
    }
}