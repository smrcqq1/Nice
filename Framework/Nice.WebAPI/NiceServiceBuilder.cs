using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Nice.WebAPI;
using System.Reflection;

namespace Nice
{
    internal class NiceServiceBuilder : INiceServiceBuilder
    {
        #region 构造函数
        public IServiceCollection Services { get;private set; }
        public NiceServiceBuilder(IServiceCollection services)
        {
            this.Services = services;
            services.AddMvcCore(op => {
                //op.Filters.Add<ModelValidator>();
            });
            //services.Configure<ApiBehaviorOptions>(op => op.SuppressModelStateInvalidFilter = true);

            //注册文件管理服务,默认注册，区别在于是否用controller暴露给前端
            services.AddScoped<IFileAPI,FileService>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }
        #endregion 构造函数

        public INiceServiceBuilder AddServices<TContracts, TService>(Type typeofInterface) where TService : class, TContracts
        {
            AddServices(typeofInterface, typeof(TContracts).Assembly, typeof(TService).Assembly);
            return this;
        }

        public INiceServiceBuilder AddServices<TContracts, TService>() where TService : class, TContracts
        {
            AddServices(typeof(IAPI), typeof(TContracts).Assembly, typeof(TService).Assembly);
            return this;
        }

        /// <summary>
        /// 从两个Assembly中自动检索需要注册的服务依赖
        /// </summary>
        /// <param name="services"></param>
        /// <param name="typeofInterface">注入的类型</param>
        /// <param name="contractsAssembly"></param>
        /// <param name="serviceAssembly"></param>
        /// <returns></returns>
        /// <remarks>
        /// 不公开是为了避免乱注册
        /// </remarks>
        IServiceCollection AddServices(Type typeofInterface, Assembly contractsAssembly, Assembly serviceAssembly)
        {
            //这个用来通知ASPNETCORE，Controllers集合有更新
            Services.AddSingleton<DynamicChangeTokenProvider>()
                    .AddSingleton<IActionDescriptorChangeProvider>(provider => provider.GetRequiredService<DynamicChangeTokenProvider>());
            //从两个Assembly中自动检索需要注册的服务依赖
            var contracts = contractsAssembly.GetTypes().Where(o => o.IsInterface && typeofInterface.IsAssignableFrom(o));
            var _services = serviceAssembly.GetTypes();
            foreach (var contract in contracts)
            {
                var service = _services.Where(o => o.IsClass && !o.IsAbstract && !o.IsInterface && contract.IsAssignableFrom(o)).FirstOrDefault();
                if (service == null)
                {
                    continue;
                }
                //if (typeof(Signs.ITransient).IsAssignableFrom(service))
                //{
                //    Services.AddTransient(contract, service);
                //    continue;
                //}
                //else if (typeof(Signs.ISingleton).IsAssignableFrom(service))
                //{
                //    Services.AddSingleton(contract, service);
                //    continue;
                //}
                Services.AddScoped(contract, service);
            }
            return Services;
        }
    }
}
