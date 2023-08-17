using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Nice.WebAPI
{
    public static class Extentions
    {
        /// <summary>
        /// 添加Nice封装的Service常用功能，如请求参数自动检查、服务自动注册等
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static INiceServiceBuilder UseNice(this IServiceCollection services)
        {
            var builder = new NiceServiceBuilder(services);
            return builder;
        }
        /// <summary>
        /// 添加Nice封装的Controller常用功能，如通用异常处理、自动生成Controller代理等
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static INiceControllerBuilder UseNice(this IApplicationBuilder app)
        {
            return new NiceControllerBuilder(app);
        }
    }
}