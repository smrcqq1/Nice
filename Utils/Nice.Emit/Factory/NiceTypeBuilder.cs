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
    /// 
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    internal abstract class NiceTypeBuilder :  ITypeProxyBuilder
    {
        #region 构造函数
        public NiceTypeBuilder(ProxyFactory proxyBuilder, Type interfaceType, Type? extendType = null)
        {
            _proxyBuilder = proxyBuilder;
            _interfaceType = interfaceType;
            _extendType = extendType;
            var typeName = interfaceType.Name.TrimStart('I');
            _typeBuilder = proxyBuilder._moduleBuilder.DefineType(typeName, TypeAttributes.Public | TypeAttributes.UnicodeClass, extendType);
            _typeBuilder.AddInterfaceImplementation(_interfaceType);
            _proxyBuilder.OnCreateType?.Invoke(_typeBuilder, interfaceType);
        }
        #endregion 构造函数

        #region 字段
        protected readonly ProxyFactory _proxyBuilder;
        protected readonly Type _interfaceType;
        protected TypeBuilder _typeBuilder;
        protected Type? _extendType;
        #endregion 字段

        #region 实现接口
        public abstract Type Create();
        #endregion 实现接口

        #region 辅助方法
        #region 为TypeBuilder增加构造函数
        public virtual  ITypeProxyBuilder AddConstructor(params Type[] constructorArguments)
        {
            foreach (var arg in constructorArguments)
            {
                var constructorBuilder = _typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, constructorArguments);
                var field = _typeBuilder.DefineField(Guid.NewGuid().ToString(),arg,FieldAttributes.InitOnly);
                // Generate IL for the method. The constructor stores its argument in the private field.
                var myConstructorIL = constructorBuilder.GetILGenerator();
                myConstructorIL.Emit(OpCodes.Ldarg_0);
                myConstructorIL.Emit(OpCodes.Ldarg_1);
                myConstructorIL.Emit(OpCodes.Stfld, field);
                myConstructorIL.Emit(OpCodes.Ret);
            }
            return this;
        }
        #endregion

        #region 创建指定接口的动态代理
        /// <summary>
        /// 创建指定接口的动态代理,其对应的实现类由nativeType指定
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        public TypeBuilder CreateDynamicProxy(ModuleBuilder moduleBuilder, Type interfaceType, Type nativeType, IEnumerable<MethodInfo> methods)
        {
            string typeName = interfaceType.Name + Guid.NewGuid().ToString();

            var myType = moduleBuilder.DefineType(typeName, TypeAttributes.Public | TypeAttributes.UnicodeClass);

            foreach (var method in methods)
            {
                var parameters = method.GetParameters();

                var parameterTypes = GetParameterTypes(parameters);

                var mBuilder = myType.DefineMethod(method.Name
                                           , MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.NewSlot | MethodAttributes.HideBySig | MethodAttributes.Final
                                           , method.ReturnType
                                           , parameterTypes);

                var il = mBuilder.GetILGenerator();

                //!!!!!Remeber, The zero of index is current instance's reference.!!!!!
                for (var i = 1; i <= parameters.Length; i++)
                {
                    il.Emit(OpCodes.Ldarg, i);
                }
                var targetMethod = nativeType.GetMethod(method.Name, parameterTypes);
                if (targetMethod == null)
                {
                    throw new InvalidOperationException($"生成{typeName}.{method.Name}的动态实现失败");
                }
                il.Emit(OpCodes.Call, targetMethod);
                if (method.ReturnType != typeof(void))
                {
                    var v = il.DeclareLocal(method.ReturnType);
                    il.Emit(OpCodes.Stloc_0, v);
                    il.Emit(OpCodes.Ldloc_0, v);
                }
                il.Emit(OpCodes.Ret);
            }
            return myType;
        }
        #endregion 创建指定接口的动态代理

        #region 获取参数集的参数类型集合
        /// <summary>
        /// 获取参数集的参数类型集合
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static Type[] GetParameterTypes(IList<ParameterInfo> parameters)
        {
            if (parameters.Count > 0)
            {
                return parameters.Select(x => x.ParameterType).ToArray();
            }
            return Type.EmptyTypes;
        }
        #endregion 获取参数集的参数类型集合
        #endregion 辅助方法
    }
}
