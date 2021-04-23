#region using
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using Test.Database;
#endregion using
namespace Test.Web
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddScoped<Contracts.IStudentService,Services.StudentService>();
            //services.AddSingleton<ICache,TestCache>();
            //services.UseNice()
            //    .UseStaticize();
            services
                .UseEFCore<TestDbContext>(o =>
                {
                    o.UseMySql("data source=localhost;database=testorm; uid=root;pwd=q111111;");
                })
                //.UseEFCore<TestDbContext2>(o =>
                //{
                //    o.UseMySql("data source=localhost;database=testorm2; uid=root;pwd=q111111;");
                //})
                ;
            services.AddResponseCaching();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseResponseCaching();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
    public class TestCache
    {
        public IActionResult Get(string key)
        {
            if (!Caches.ContainsKey(key))
            {
                return null;
            }
            return Caches[key];
        }
        public bool Set(string key, IActionResult data, int expireTime = -1)
        {
            if (Caches.ContainsKey(key))
            {
                Caches[key] = data;
            }
            else
            {
                Caches.Add(key,data);
            }
            return true;
        }
        static readonly Dictionary<string, IActionResult> Caches = new Dictionary<string, IActionResult>();
    }
    public class Cache : Attribute,IActionFilter
    {
        TestCache cache = new TestCache();
        public void OnActionExecuted(ActionExecutedContext context)
        {
            cache.Set(GetKey(context.HttpContext),context.Result);
        }
        string GetKey(HttpContext httpContext)
        {
            var key = $"{httpContext.Request.Path}?{httpContext.Request.QueryString}";
            return key;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if(context.HttpContext.Request.Method != "GET")
            {
                return;
            }
            var res = cache.Get(GetKey(context.HttpContext));
            if (res != null)
            {
                //var tmp = res as ObjectResult;
                //tmp.Value = "ÎÒÀ´×Ô»º´æ:" + Newtonsoft.Json.JsonConvert.SerializeObject( tmp.Value);
                context.Result = res;
            }
        }
    }
}