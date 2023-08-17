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
                //�������������֤ʧ��ʱ����η��ش����ǰ��
                //.SetModelValidator(errors => {
                //    return "error";
                //})

                .AddServices<ITestAPI, TestService>();
            //����ֱ��ע��ʹ�û��棬Ȼ����Ҫ��ƽ��һ��,������Ա��ҵ���߼����ֶ�����
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

            //ͨ��Nice��ܽ����Զ�����,����ֻ��ͨ����Ŀ����ӿ���Ͻ��л������ã���ҵ�񿪷���Ա���������Щ���ݱ�����Ӷ�����ҵ�������ȶ�

            //���ýӿ����ݵĻ���
            builder.AddCaching(new StaticzeCachingProvider())

                //���Զ�����ã����γɶ༶�������
                //ʹ��nice����ṩ���ڴ滺�����
                .AddMemoryCaching()
                //����ASPNETCORE�Դ���ResponseCaching
                .AddResponseCaching()
                //�����Զ���Ļ������
                .AddCaching(new RedisCachingProvider())

                //�Զ�ΪICachingTestAPI�еĵ�GetStudentInfo�ӿ����û���
                .Set<ICachingTestAPI>(o => o.GetStudentInfo)
                    //��ICachingTestAPI�е�EditStudentInfo�ӿڱ��ɹ����õ�ʱ���Զ�������һ��ָ���ĽӿڵĻ�������
                    .When<ICachingTestAPI>(o => o.EditStudentInfo)
                    //������Ҳ�ᰴ��ʱ�����
                    .Expire(20)

                //����������������
                .Set<ICachingTestAPI>(o=>o.GetStudentInfo)

                //����Ϊ��д��
                //�趨GetStudentInfo���չ���ʱ����л���
                .Set<ICachingTestAPI>(o => o.GetStudentInfo,20)
                //�趨GetStudentInfo�����ݻᱻ���棬����ֻ�е�EditStudentInfo�����õ�ʱ����»���
                .Set<ICachingTestAPI>(o=>o.GetStudentInfo,o=>o.EditStudentInfo);

            //������ͬ���ķ�ʽ���������ݿ����ݵĻ���
            builder.AddCaching<Student>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}