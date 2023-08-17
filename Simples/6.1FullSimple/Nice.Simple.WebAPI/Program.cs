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
    //todo  ��Ӧ����ͳһ�ĳ�ʼ���ݴ�����
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
//todo ���ڽӿ���Ȩ����ķ�װδ����
app.UseAuthorization();
//ʹ��Nice���
var nice = app.UseNice();
//ʹ��Nice�ṩ���ļ��ϴ����ؽӿ�
nice.AddFileHandler();
//nice��ܷ������ݵķ����ͨ��httpcode�����Ƿ�ɹ���body�������ݣ��򵥸ɾ�û���κζ�����Ϣ��������ɾ�Ķ�ֻ����httpcode=200��bodyû���κ�����
//������ԶԽӿڷ������ݽ���ͳһ���ã��Ա㷵�ر���˾�Լ���������
nice.SetResponseSpecification(msg => new { code = 200,data = msg});
//ʹ��ͳһ�쳣����
nice.AddGlobalExceptionHandler();
//���ýӿڻ���
nice.AddMemoryCaching()
    //ָ��Student/AggrAsync��PageAsync�����ӿڵ����ݻᱻ����
    .Set<IStudentAPI>(o=>o.InfoAsync)
    //�����������»������������һ����ɼ��ɸ���InfoAsync�Ļ���
    //���޸�ѧ����Ϣ�ӿڱ�����ʱ�Զ����»���
    //todo �����Զ����������ӿڵ�ID����PostAsync��һ��ѧ��id����InfoAsync�ж�Ӧid�Ļ������ݱ�����/���
    .When(o=>o.PostAsync)
    ///����20�����
    .Expire(20)
    //��������
    .SlidingExpire()
    //����ʦapi�е�ָ���ӿڱ�����ʱ��ҲҪ���´˽ӿ�
    //todo ����Ӧ������Ҫ������Щ����,������ʱ�����Ӻζ���
    .When<ITeacherAPI>(o=>o.AddAsync);

//Ϊ��Լ���Զ�����Controller
//todo ������controller��ʱ���Զ���������·������ɽ�ȥ
nice.AddControllers<IStudentAPI>();

app.MapControllers();

app.Run();
