using Microsoft.EntityFrameworkCore;
using Nice;
using Nice.Caching;
using Nice.Simple.Contracts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
Nice.EFCore.AutoDbContext.EnsureCreated = true;
builder.Services.UseAutoEFCore(op => {
    op.UseInMemoryDatabase("test");
    //op.UseSqlite("Data Source=simple.db");
})
    //todo  还应该有统一的初始数据处理方法
    .SetTables(typeof(Nice.Simple.DataModel.Student).Assembly);

builder.Services.UseNice()
    .AddServices<IStudentAPI,Nice.Simple.Service.StudentService>()
    .AddServices<ITeacherAPI,Nice.Simple.Service.TeacherService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//todo 关于接口授权方面的封装未进行
app.UseAuthorization();
//使用Nice框架
var nice = app.UseNice();
//使用Nice提供的文件上传下载接口
nice.AddFileHandler();
//nice框架返回数据的风格是通过httpcode返回是否成功，body返回数据，简单干净没有任何多余信息；例如增删改都只返回httpcode=200，body没有任何内容
//这里可以对接口返回数据进行统一配置，以便返回本公司自己风格的数据
nice.SetResponseSpecification(msg => new { code = 200,data = msg});
//使用统一异常处理
nice.AddGlobalExceptionHandler();
//配置接口缓存
nice.AddMemoryCaching()
    //指定Student/AggrAsync和PageAsync两个接口的数据会被缓存
    .Set<IStudentAPI>(o=>o.InfoAsync)
    //以下三个更新缓存的条件任意一个达成即可更新InfoAsync的缓存
    //当修改学生信息接口被调用时自动更新缓存
    //todo 这里自动关联两个接口的ID，即PostAsync了一个学生id，则InfoAsync中对应id的缓存数据被更新/清除
    .When(o=>o.PostAsync)
    ///设置20秒过期
    .Expire(20)
    //滑动过期
    .SlidingExpire()
    //当教师api中的指定接口被调用时，也要更新此接口
    //todo 这里应该配置要更新哪些缓存,即更新时参数从何而来
    .When<ITeacherAPI>(o=>o.AddAsync);

//为契约层自动生成Controller
//todo 在生成controller的时候，自动将缓存更新方案生成进去
nice.AddControllers<IStudentAPI>();

app.MapControllers();

app.Run();
