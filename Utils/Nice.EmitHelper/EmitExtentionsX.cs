#region using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

#endregion using

namespace Nice
{
    public class EmitExtentionsX
    {
        private readonly AssemblyBuilder _builder;
        private readonly ModuleBuilder _moduleBuilder;

        public EmitExtentionsX()
        {
            AssemblyName aName = new AssemblyName("DynamicProxyAssembly");

            _builder =
                AssemblyBuilder.DefineDynamicAssembly(
                    aName,
                    AssemblyBuilderAccess.Run);

            _moduleBuilder = _builder.DefineDynamicModule(aName.Name + ".dll");
        }

        public Type CreateProxy<T>() where T : class
        {
            // public class Namespace_XXXProxy
            // {
            //     private readonly IProxyProvider _provider;
            //
            //     public Namespace_XXXProxy(IProxyProvider a)
            //     {
            //         _provider = a;
            //     }
            //
            //     public async Task<string> Method1(string arg1, string arg2)
            //     {
            //         List<KeyValuePair<string, object>> argumentBox = new List<KeyValuePair<string, object>>();
            //
            //         KeyValuePair<string, object> arg_1 = new KeyValuePair<string, object>("arg1", arg1);
            //         argumentBox.Add(arg_1);
            //
            //         KeyValuePair<string, object> arg_2 = new KeyValuePair<string, object>("arg1", arg1);
            //         argumentBox.Add(arg_2);
            //
            //         var val = await _provider.Invoke("Method1", argumentBox);
            //         var resultVal = TypeConveter.As<string>(val);
            //         return resultVal;
            //     }
            // }

            var typed = typeof(T);
            var typedMethods = typed.GetMethods(BindingFlags.Instance | BindingFlags.Default | BindingFlags.Public);

            TypeBuilder typeBuilder = _moduleBuilder.DefineType($"{typed.Namespace.Replace(".", "_")}_{typed.Name}Proxy");

            typeBuilder.AddInterfaceImplementation(typed);

            var attrs = MethodAttributes.Public
                        | MethodAttributes.Virtual
                        | MethodAttributes.NewSlot
                        | MethodAttributes.HideBySig
                        | MethodAttributes.Final;

            var providerField = typeBuilder.DefineField("_provider", typeof(IProxyProvider),
                FieldAttributes.Private | FieldAttributes.InitOnly);

            var constructorBuilder = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.HasThis,
                new[] {typeof(IProxyProvider)});

            var constructorIL = constructorBuilder.GetILGenerator();
            constructorIL.Emit(OpCodes.Ldarg_0);
            constructorIL.Emit(OpCodes.Stloc, providerField);
            constructorIL.Emit(OpCodes.Ret);

            foreach (var method in typedMethods)
            {
                var parameters = method.GetParameters();
                var parameterTypes = parameters.Select(x => x.ParameterType).ToArray();

                MethodBuilder methodBuilder =
                    typeBuilder.DefineMethod(method.Name, attrs, method.ReturnType, parameterTypes);

                var IL = methodBuilder.GetILGenerator();

                var providerTyped = typeof(IProxyProvider);
                var providerHandler = IL.DeclareLocal(providerTyped);

                IL.Emit(OpCodes.Ldarg_0);
                IL.Emit(OpCodes.Stloc, providerHandler);

                var argumentBoxTyped = typeof(List<KeyValuePair<string, object>>);
                var argumentBox = IL.DeclareLocal(argumentBoxTyped);

                var index = 0;
                foreach (var p in parameters)
                {
                    var argumentItemTyped = typeof(KeyValuePair<string, object>);
                    var arg = IL.DeclareLocal(argumentItemTyped);
                    IL.Emit(OpCodes.Ldstr, p.Name);
                    IL.Emit(OpCodes.Ldarg, index);
                    IL.Emit(OpCodes.Newobj, argumentItemTyped);
                    IL.Emit(OpCodes.Stloc, arg);

                    IL.Emit(OpCodes.Ldloc, arg);
                    IL.Emit(OpCodes.Ldloc, argumentBox);
                    IL.Emit(OpCodes.Call, argumentBoxTyped.GetMethod("Add"));

                    index++;
                }

                var val = IL.DeclareLocal(typeof(object));

                IL.Emit(OpCodes.Ldloc, method.Name);
                IL.Emit(OpCodes.Ldloc, argumentBox);
                IL.Emit(OpCodes.Ldloc, providerHandler);
                IL.Emit(OpCodes.Call, providerTyped.GetMethod("Invoke"));
                IL.Emit(OpCodes.Stloc, val);
                
                var resultVal = IL.DeclareLocal(method.ReturnType);
                IL.Emit(OpCodes.Ldloc, method.ReturnType);
                IL.Emit(OpCodes.Ldloc, val);
                IL.Emit(OpCodes.Call, typeof(TypeConveter).GetMethod("As"));
                IL.Emit(OpCodes.Stloc, resultVal);
                
                IL.Emit(OpCodes.Ldloc, resultVal);
                IL.Emit(OpCodes.Ret);
            }
            
            return typeBuilder.CreateType();
        }
    }
}