using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Reflection.Emit;
using System.Reflection;

namespace Nice.RPC
{
    public class Proxy
    {
        //private static readonly ProxyGenerator _proxyGenerator = new ProxyGenerator();
        //private static ConcurrentDictionary<string, object> _services = new ConcurrentDictionary<string, object>();


        //public static Type Get<T>(IRPC rPC) where T : class
        //{
        //    //Emit方式
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
        static Type build(Type targetType)
        {
            if (!targetType.IsInterface)
            {
                NiceException.Throw($"只能代理Interface,无法代理class: {targetType.FullName}");
            }
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
            //新类型的名称：随便定一个
            var newTypeName = targetType.Name + "Imp";
            //新类型的属性：要创建的是Class，而非Interface，Abstract Class等，而且是Public的
            TypeAttributes newTypeAttribute = TypeAttributes.Class | TypeAttributes.Public;

            //得到类型生成器
            var typeBuilder = modBuilder.DefineType(newTypeName, newTypeAttribute, null, new Type[] { targetType });

            //以下将为新类型声明方法：新类型应该override基类型的所有public方法

            //得到基类型的所有方法
            var targetMethods = targetType.GetMethods();

            //遍历各个方法，对于Virtual的方法，获取其签名，作为新类型的方法
            foreach (var targetMethod in targetMethods)
            {
                //得到方法的各个参数的类型
                var paramType = targetMethod.GetParameters().Select(o=>o.ParameterType).ToArray();

                //传入方法签名，得到方法生成器
                var methodBuilder = typeBuilder.DefineMethod(targetMethod.Name, MethodAttributes.Public | MethodAttributes.Virtual, targetMethod.ReturnType, paramType);

                //由于要生成的是具体类，所以方法的实现是必不可少的。而方法的实现是通过Emit IL代码来产生的

                //得到IL生成器
                var ilGen = methodBuilder.GetILGenerator();
                //以下三行相当于：{Console.Writeln("I'm "+ targetMethod.Name +"ing");}
                ilGen.Emit(OpCodes.Ldstr, "I'm " + targetMethod.Name + "ing");
                ilGen.Emit(OpCodes.Call, typeof(Console).GetMethod("WriteLine", new Type[] { typeof(String) }));
                ilGen.Emit(OpCodes.Ret);
            }
            //真正创建，并返回
            return typeBuilder.CreateType();
        }
        #endregion Emit方式
    }
}