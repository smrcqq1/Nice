#region using
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

#endregion using
namespace Nice.Tests.Host
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    internal class HostBase : ITestHost
    {
        static HostBase()
        {
            ServiceCollection = new ServiceCollection(); ;
        }
        static readonly IServiceCollection ServiceCollection;
        static IServiceProvider serviceProvider;

        public IServiceCollection Services => ServiceCollection;

        public TService GetService<TService>()
        {
            if(serviceProvider == null)
            {
                serviceProvider = ServiceCollection.BuildServiceProvider();
            }
            var service = serviceProvider.GetService<TService>();
            return service!;
        }

        public ITestHost AddScoped<TService, TImplement>() where TService : class where TImplement : class,TService
        {
            ServiceCollection.AddScoped<TService, TImplement>();
            return this;
        }
        public ITestHost AddSingleton<TService, TImplement>() where TService : class where TImplement : class, TService
        {
            ServiceCollection.AddSingleton<TService, TImplement>();
            return this;
        }
        public ITestHost AddTransient<TService, TImplement>() where TService : class where TImplement : class, TService
        {
            ServiceCollection.AddTransient<TService, TImplement>();
            return this;
        }

        public ITestHost AddTransient(Type type1, Type type2)
        {
            ServiceCollection.AddTransient(type1,type2);
            return this;
        }
    }
}
