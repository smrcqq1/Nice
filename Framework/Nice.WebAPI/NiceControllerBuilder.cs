using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.Reflection;
using System.Reflection.Emit;

namespace Nice.WebAPI
{
    internal class NiceControllerBuilder : INiceControllerBuilder
    {
        public NiceControllerBuilder(IApplicationBuilder app)
        {
            this.app = app;
        }

        internal readonly IApplicationBuilder app;
        /// <summary>
        /// 查找指定的Assembly，为其中所有符合要求的接口生成Controller层代理
        /// </summary>
        /// <param name = "assembly" ></ param >
        private Assembly MapAssembly(Assembly assembly, string front = "")
        {
            //new ControllerProxy(app.ApplicationServices.GetService<IServiceProvider>())
            var factory = Nice.Emit.Extentions.GetBuilder("Controllers" + Guid.NewGuid());
            factory.OnCreateMethod = (builder, method) => {
                var signs = method.GetCustomAttributes();
                var httpmethodtype = GetCustomAttributeBuilder(signs, b => {
                    builder.SetCustomAttribute(b);
                }, () => method.Name.RemoveEnd("Async"));
                return (paraBuilder, MethodInfo, parameter) => {
                    if (httpmethodtype != null)
                    {
                        paraBuilder.SetCustomAttribute(SignToAttribute(httpmethodtype));
                    }
                };
            };
            factory.OnCreateType = (builder, type) => {
                var signs = type.GetCustomAttributes();
                GetTypeCustomAttributeBuilder(signs, b => {
                    builder.SetCustomAttribute(b);
                }, () => front + type.Name.RemoveStart("I").RemoveEnd("API"));
            };

            var contracts = assembly.GetTypes().Where(o => o.IsInterface && typeof(Nice.IAPI).IsAssignableFrom(o));
            if (!contracts.Any())
            {
                throw new DebugException("未发现任何接口,请检查您的接口是否标记了Nice.Signs.IAPI");
            }
            var result = factory.CreateInjectTypes(contracts, typeof(ControllerBase));
            return result;
        }
        static Type? GetCustomAttributeBuilder(IEnumerable<Attribute> signs, Action<CustomAttributeBuilder> action, Func<string>? getName = null)
        {
            if (!signs.Any())
            {
                action(SignToAttribute(typeof(HttpPostAttribute)));
                action(SignToAttribute(typeof(RouteAttribute), getName!.Invoke()));
                return typeof(FromBodyAttribute);
            }
            Type? httpMethodType = null;
            var method = signs.Where(sign => sign is Attributes.HttpMethodAttribute).FirstOrDefault();
            if(method != null)
            {
                var httpMethod = method as Attributes.HttpMethodAttribute;
                Type? attr = null;
                switch (httpMethod!.HttpMethodType)
                {
                    case Attributes.HttpMethodType.HttpPost:
                        httpMethodType = typeof(FromBodyAttribute);
                        attr = typeof(HttpPostAttribute);
                        break;
                    case Attributes.HttpMethodType.HttpPut:
                        httpMethodType = typeof(FromBodyAttribute);
                        attr = typeof(HttpPutAttribute);
                        break;
                    case Attributes.HttpMethodType.HttpDelete:
                        httpMethodType = typeof(FromQueryAttribute);
                        attr = typeof(HttpDeleteAttribute);
                        break;
                    case Attributes.HttpMethodType.HttpGet:
                        httpMethodType = typeof(FromQueryAttribute);
                        attr = typeof(HttpGetAttribute);
                        break;
                    default: throw new DebugException("不支持的HttpMethod");
                };
                action(SignToAttribute(attr));
            }
            else
            {
                action(SignToAttribute(typeof(HttpPostAttribute)));
                httpMethodType = typeof(FromBodyAttribute);
            }
            var route = signs.Where(sign => sign is Attributes.RouteAttribute).FirstOrDefault();
            if(route != null)
            {
                action(SignToAttribute(typeof(RouteAttribute), (route as Attributes.RouteAttribute)!.Template!));
            }
            else
            {
                action(SignToAttribute(typeof(RouteAttribute), getName!.Invoke()));
            }
            return httpMethodType;
        }
        static void GetTypeCustomAttributeBuilder(IEnumerable<Attribute> signs, Action<CustomAttributeBuilder> action, Func<string>? getName = null)
        {
            if (!signs.Any())
            {
#pragma warning disable CS8604 // 引用类型参数可能为 null。
                action(SignToAttribute(typeof(RouteAttribute), getName?.Invoke()));
#pragma warning restore CS8604 // 引用类型参数可能为 null。
                return ;
            }
            var route = signs.Where(sign => sign is Attributes.RouteAttribute).FirstOrDefault();
            if (route != null)
            {
                action(SignToAttribute(typeof(RouteAttribute), (route as Attributes.RouteAttribute)!.Template!));
            }
            else
            {
#pragma warning disable CS8604 // 引用类型参数可能为 null。
                action(SignToAttribute(typeof(RouteAttribute), getName?.Invoke()));
#pragma warning restore CS8604 // 引用类型参数可能为 null。
            }
        }
        static CustomAttributeBuilder SignToAttribute(Type type, params string[]? paras)
        {
            var types = Type.EmptyTypes;
            if (paras != null && paras.Length > 0)
            {
                types = paras.Select(o => typeof(string)).ToArray();
            }
            var ctor = type.GetConstructor(types);
            return new CustomAttributeBuilder(ctor, paras);
        }
        ///// <summary>
        ///// 为接口生成Controller层代理
        ///// </summary>
        ///// <param name="assembly"></param>
        //public static object MapController<T>(Type parentType, ControllerProxy controllerProxy) where T:class
        //{
        //    var builder = new ProxyGenerator();
        //    var result = builder.CreateInterfaceProxyWithoutTarget<T>(controllerProxy);
        //    return result;
        //}
        public INiceControllerBuilder AddControllers(Assembly assembly, string front = "")
        {
            assembly = MapAssembly(assembly,front);
            AddApplicationPartManager(assembly);
            return this;
        }
        private INiceControllerBuilder AddApplicationPartManager(Assembly assembly)
        {
            var tokenProvider = app.ApplicationServices.GetService<DynamicChangeTokenProvider>();
            var applicationPartManager = app.ApplicationServices.GetService<ApplicationPartManager>();
            applicationPartManager!.ApplicationParts.Add(new AssemblyPart(assembly));
            tokenProvider!.NotifyChanges();
            return this;
        }
        public INiceControllerBuilder AddControllers<T>(string front = "")
        {
            var assembly = typeof(T).Assembly;
            AddControllers(assembly, front);
            return this;
        }
        public INiceControllerBuilder AddGlobalExceptionHandler()
        {
            app.UseMiddleware<GlobalExceptionMiddleware>();
            return this;
        }
        /// <summary>
        /// 使用通用文件处理，包括上传、下载、删除、移动和更新等常用文件操作
        /// </summary>
        /// <returns></returns>
        public INiceControllerBuilder AddFileHandler(string front = "")
        {
            var assembly = MapAssembly(typeof(IFileAPI).Assembly, front);
            AddApplicationPartManager(assembly);
            return this;
        }

        public INiceControllerBuilder SetModelValidator(Func<string[], object> errorToResult)
        {
            ModelValidator.ErrorToResultFunc = errorToResult;
            return this;
        }
        public List<ICachingBuilder> CachingBuilders { get; set; } = new List<ICachingBuilder>();

        public INiceControllerBuilder AddCaching(ICachingBuilder builder)
        {
            CachingBuilders.Add(builder);
            return this;
        }
        Func<object, object> messageHandler;
        public INiceControllerBuilder SetResponseSpecification(Func<object, object> messageHandler)
        {
            this.messageHandler = messageHandler;
            //todo 未完成自定义返回格式的处理
            return this;
        }
    }
}