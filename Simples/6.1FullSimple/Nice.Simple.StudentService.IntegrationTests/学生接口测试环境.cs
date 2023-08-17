using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Nice.Simple.StudentService.IntegrationTests;
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
public class 学生接口测试环境 : TestBase
{
    //这个方式要弃用，不太方便
    protected override void Setup(IServiceCollection services)
    {
        base.Setup(services);
        services.AddTransient<IStudentAPI, Service.StudentService>();
        Nice.EFCore.AutoDbContext.SetTables(typeof(Simple.DataModel.Student));
    }
    #region 准备测试环境
    public static readonly Guid 在职教师ID = Guid.NewGuid();
    public static readonly Guid 已离职教师ID = Guid.NewGuid();
    //Teacher相关的功能不属于本人负责开发，所以要Mock它来测试，从而避免等待Teacher模块的开发
    public ITeacherAPI MockTeacherService
    {
        get
        {
            var teacher = MockService<ITeacherAPI>();
            //模拟id为1的老师是在职状态
            teacher.Setup(o => o.CheckStatusAsync(在职教师ID))
                .SetReturns(Contracts.Enums.TeacherStatus.在职);
            //模拟id为2的老师是已离职状态
            teacher.Setup(o => o.CheckStatusAsync(已离职教师ID))
                .SetReturns(Contracts.Enums.TeacherStatus.已离职);
            return teacher.MockedClass;
        }
    }
    public IStudentAPI StudentService
    {
        get
        {
            return new Service.StudentService(DataProvider, MockTeacherService);
        }
    }
    #endregion 准备测试环境

}