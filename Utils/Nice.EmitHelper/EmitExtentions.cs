#region using
using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
#endregion using
namespace Nice
{
    /// <summary>
    /// 对EMIT的一般封装和扩展
    /// </summary>
    public static class EmitExtentions
    {
        #region 获取一个指定类型的代理类
        /// <summary>
        /// 获取一个指定类型的代理类
        /// </summary>
        /// <param name="type"></param>
        public static Type Proxy(this Type type)
        {
            var builder = type.Assembly.Proxy();
            var proxy = builder.DefineType(type);
            //Acb.MicroService.Client.Proxy.AsyncDispatchProxyGenerator.CreateProxyType(type);
            return proxy;
        }
        #endregion 获取一个指定类型的代理类

        #region 获取一个Assembly的代理Assembly
        /// <summary>
        /// 获取一个Assembly的代理Assembly
        /// </summary>
        /// <param name="type"></param>
        public static ModuleBuilder Proxy(this Assembly assembly)
        {
            var name = assembly.GetName().Name;
            //System.Reflection.AssemblyName 是用来表示一个Assembly的完整名称的
            var assyName = new AssemblyName
            {
                //为要创建的Assembly定义一个名称（这里忽略版本号，Culture等信息）
                Name = "MyAssyFor_" + name
            };
            //获取AssemblyBuilder
            //AssemblyBuilderAccess有Run，Save，RunAndSave三个取值
            var assyBuilder =
               AssemblyBuilder.DefineDynamicAssembly(
                   assyName,
                   AssemblyBuilderAccess.Run);
            //获取ModuleBuilder，提供String参数作为Module名称，随便设一个
            var modBuilder = assyBuilder.DefineDynamicModule("MyModFor_" + name);
            //真正创建，并返回
            return modBuilder;
        }
        #endregion 获取一个Assembly的代理Assembly

        #region 生成指定类型的代理
        /// <summary>
        /// 生成指定类型的代理类型
        /// </summary>
        /// <param name="modBuilder"></param>
        /// <param name="targetType"></param>
        /// <returns></returns>
        public static Type DefineType(this ModuleBuilder modBuilder,Type targetType)
        {
            //新类型的名称：随便定一个
            var newTypeName = targetType.Name + "Imp";
            //新类型的属性：要创建的是Class，而非Interface，Abstract Class等，而且是Public的
            var newTypeAttribute = TypeAttributes.Class | TypeAttributes.Public;
            //得到类型生成器
            var typeBuilder = modBuilder.DefineType(newTypeName, newTypeAttribute, null, new Type[] { targetType });
            //以下将为新类型声明方法：新类型应该override基类型的所有public方法
            //得到基类型的所有方法
            var targetMethods = targetType.GetMethods();
            //遍历各个方法，对于Virtual的方法，获取其签名，作为新类型的方法
            foreach (var targetMethod in targetMethods)
            {
                typeBuilder.DefineMethod(targetMethod);
            }
            return typeBuilder.CreateType();
        }
        #endregion 生成指定类型的代理

        #region 生成指定方法的代理
        public static void DefineMethod(this TypeBuilder typeBuilder,MethodInfo targetMethod)
        {
            //得到方法的各个参数的类型
            var paramType = targetMethod.GetParameters().Select(o => o.ParameterType).ToArray();
            //传入方法签名，得到方法生成器
            var methodBuilder = typeBuilder.DefineMethod(targetMethod.Name, MethodAttributes.Public | MethodAttributes.Virtual, targetMethod.ReturnType, paramType);

            //由于要生成的是具体类，所以方法的实现是必不可少的。而方法的实现是通过Emit IL代码来产生的

            //得到IL生成器
            var ilGen = methodBuilder.GetILGenerator();
#warning IL生成方法尚未实现
            //以下三行相当于：{Console.Writeln("I'm "+ targetMethod.Name +"ing");}
            ilGen.Emit(OpCodes.Ldarga_S, 1);
            ilGen.Emit(OpCodes.Call, typeof(int).GetMethod("ToString"));

            ilGen.Emit(OpCodes.Ret);
        }
        #endregion 生成指定方法的代理
    }
}