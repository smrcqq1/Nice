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
    internal class InjectTypeBuilder : NiceTypeBuilder, IInjectTypeBuilder
    {
        #region 构造函数
        public InjectTypeBuilder(ProxyFactory proxyBuilder, Type interfaceType, Type? extendType = null) : base(proxyBuilder, interfaceType, extendType)
        {
        }
        #endregion 构造函数

        /// <summary>
        /// 创建指定类型的代理类
        /// </summary>
        /// <returns>代理类</returns>
        /// <remarks>
        /// 1.如果T是接口，则通过构造函数注入service的方式调用原接口的实现类
        /// 2.如果T是实现类，则通过构造函数注入的方式直接调用原类对应的原方法
        /// 3.通过T上的Signs.ILifeCycle相关标记决定生命周期，如果没有默认为Scoped生命周期
        /// </remarks>
        public override Type Create()
        {
            //注入类的代理需要生成一个构造函数，以便注入接口的实现
            var field = AddConstructor(_interfaceType);
            var methods = _interfaceType.GetMethods();

            var injectType = CreateInjectType(methods, field);
            return injectType;
        }

        #region 为TypeBuilder增加构造函数
        /// <summary>
        /// 默认类型数组
        /// </summary>
        static readonly Type[] Types0 = new Type[0];
        FieldInfo AddConstructor(Type constructorArguments)
        {
            var constructorBuilder = _typeBuilder.DefineConstructor(
                MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName,
                CallingConventions.HasThis,
                new Type[] { constructorArguments }
                );
            var field = _typeBuilder.DefineField(
                "_service",
                constructorArguments, FieldAttributes.InitOnly | FieldAttributes.Private
                );
            // Generate IL for the method. The constructor stores its argument in the private field.
            var il = constructorBuilder.GetILGenerator();
            //查找父类的无参构造函数
            var baseCtor = _extendType?.GetConstructor(Types0);
            if (baseCtor != null)
            {
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Call, baseCtor);
            }
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Stfld, field);
            il.Emit(OpCodes.Ret);
            return field;
        }
        #endregion

        #region 创建指定接口的动态代理
        /// <summary>
        /// 创建指定接口的动态代理,其对应的实现类由nativeType指定
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        private Type CreateInjectType(IEnumerable<MethodInfo> methods, FieldInfo field)
        {
            foreach (var method in methods)
            {
                var parameters = method.GetParameters();
                var parameterTypes = GetParameterTypes(parameters);

                var mBuilder = _typeBuilder.DefineMethod(method.Name
                                           , MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.NewSlot | MethodAttributes.Virtual | MethodAttributes.Final
                                           , method.ReturnType
                                           , parameterTypes);
                var onCreateParameter = _proxyBuilder.OnCreateMethod?.Invoke(mBuilder, method);
                //处理输入参数
                var index = 0;
                if(parameters.Length > 0)
                {
                    foreach (var parameter in parameters)
                    {
                        index++;
                        var paraBuilder = mBuilder.DefineParameter(index,ParameterAttributes.In,parameter.Name);
                        onCreateParameter?.Invoke(paraBuilder,method,parameter);
                    }
                }
                //if (method.ReturnType != typeof(void))
                //{
                //    index++;
                //    mBuilder.DefineParameter(index, ParameterAttributes.Retval,"return");
                //}
                #region il方式
                var il = mBuilder.GetILGenerator();
                var targetMethod = field.FieldType.GetMethod(method.Name, parameterTypes);
                if (targetMethod == null)
                {
                    throw new InvalidOperationException($"生成{_typeBuilder.Name}.{method.Name}的动态实现失败");
                }

                //由于代理类只能调取通过构造函数注入的实现，所以第一个参数一定是该实现的字段
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldfld, field);

                //!!!!!Remeber, The zero of index is current instance's reference.!!!!!
                for (var i = 1; i <= parameters.Length; i++)
                {
                    il.Emit(OpCodes.Ldarg, i);
                }

                il.Emit(OpCodes.Callvirt, targetMethod);
                //处理返回
                if (method.ReturnType != typeof(void))
                {
                    il.Emit(OpCodes.Stloc_0);
                    var v = il.DeclareLocal(method.ReturnType);
                    il.Emit(OpCodes.Ldloc_0, v);
                }
                il.Emit(OpCodes.Ret);
                #endregion il方式
            }
            return _typeBuilder.CreateType();
        }
        #endregion 创建指定接口的动态代理
    }
}
