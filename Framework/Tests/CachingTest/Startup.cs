using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Nice.Caching;
using Nice.WebAPI;

namespace CachingTest
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CachingTest", Version = "v1" });
            });

            var builder = services.UseNice()
                //设置请求参数验证失败时，如何返回错误给前端
                //.SetModelValidator(errors => {
                //    return "error";
                //})

                .AddServices<ITestAPI, TestService>();
            //可以直接注册使用缓存，然后需要像平常一样,开发人员在业务逻辑中手动调用
            //services.AddCaching(new MemoryCachingProvider());

        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CachingTest v1"));
            //}

            app.UseRouting();
            app.UseAuthorization();

            var builder = app.UseNice()
               .AddGlobalExceptionHandler()
               .AddFileHandler()
               .AddControllers<ITestAPI>();

            //通过Nice框架进行自动管理,这样只能通过项目经理从框架上进行缓存配置，而业务开发人员无需关心哪些数据被缓存从而保持业务代码的稳定

            //配置接口数据的缓存
            builder.AddCaching(new StaticzeCachingProvider())

                //可以多次设置，以形成多级缓存机制
                //使用nice框架提供的内存缓存机制
                .AddMemoryCaching()
                //加入ASPNETCORE自带的ResponseCaching
                .AddResponseCaching()
                //加入自定义的缓存机制
                .AddCaching(new RedisCachingProvider())

                //自动为ICachingTestAPI中的的GetStudentInfo接口配置缓存
                .Set<ICachingTestAPI>(o => o.GetStudentInfo)
                    //当ICachingTestAPI中的EditStudentInfo接口被成功调用的时候，自动更新上一步指定的接口的缓存数据
                    .When<ICachingTestAPI>(o => o.EditStudentInfo)
                    //设置它也会按照时间过期
                    .Expire(20)

                //继续设置其它缓存
                .Set<ICachingTestAPI>(o=>o.GetStudentInfo)

                //以下为简化写法
                //设定GetStudentInfo按照过期时间进行缓存
                .Set<ICachingTestAPI>(o => o.GetStudentInfo,20)
                //设定GetStudentInfo的数据会被缓存，并且只有当EditStudentInfo被调用的时候更新缓存
                .Set<ICachingTestAPI>(o=>o.GetStudentInfo,o=>o.EditStudentInfo);

            //可以用同样的方式，配置数据库数据的缓存
            builder.AddCaching<Student>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}