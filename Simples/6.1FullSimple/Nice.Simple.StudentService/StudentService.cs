using Nice.Services;

namespace Nice.Simple.Service;
public class StudentService :ServiceBase<DataModel.Student>, IStudentAPI
{
    #region 构造函数
    readonly ITeacherAPI _teacherService;
    public StudentService(IDataProvider dataProvider, ITeacherAPI teacherService):base(dataProvider)
    {
        _teacherService = teacherService;
    }
    #endregion 构造函数

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="BuzinessException"></exception>
    /// <remarks>
    /// 这是主要完整的示例
    /// 1.AddStudentRequest的合法性验证由框架自动完成，不需要业务代码检查，只要能进来，一定是合法的
    /// 2.这里演示了完整的新增学生数据的业务流程，包含3个业务逻辑错误要处理
    /// 3.请注意：这里的业务依赖了TeacherService，但是完全没有对TeacherService实现的引用
    /// 4.这个代码在开发阶段只能通过下面的测试项目里的测试方法来调试，理论上这是效率最高的方式，比跑起来用前端点点点再来打断点高效得多，
    /// 如果习惯好，还能积累很多测试用例
    /// </remarks>
    public async Task<Guid> AddAsync(AddStudentRequest request)
    {
        this.For(request).NotNull("参数不能为空").Throw()
            .For(request.TeacherID).NotNull("请选择班主任")
            .For(request.Name)
            //.NotNull("学生姓名不能为空").Min("学生姓名最少输入2个字符",2).Max("学生姓名最多100个字符",100)
            //上一句可以简化为下面一句,上面三个封装用于单独的检查
            .Between("学生姓名", 2, 100)
            .Throw();
        var teacherStatus = await _teacherService.CheckStatusAsync(request.TeacherID) ?? throw new BuzinessException("不存在该教师");
        if (teacherStatus == Contracts.Enums.TeacherStatus.已离职)
        {
            throw new BuzinessException("不能为已离职教师分配学生");
        }
        var existed = await Set.Where(o => o.Name == request.Name).AnyAsync();
        if (existed)
        {
            throw new BuzinessException("学生姓名不能重复");
        }
        return await Set.AddAsync(new DataModel.Student()
        {
            Name = request.Name,
            TeacherID = request.TeacherID,
            IDCard = "IDCard",
            Image = "Image",
            Phone = "Phone"
        });
    }
    /// <remarks>
    ///这里演示当有业务逻辑错误没有处理，测试应该如何处理
    ///本来应该有两个业务逻辑错误，但是这里只写了一条
    /// </remarks>
    public async Task PostAsync(EditStudentRequest request)
    {
        var teacherStatus = await _teacherService.CheckStatusAsync(request.TeacherID);
        if (teacherStatus == Contracts.Enums.TeacherStatus.已离职)
        {
            throw new BuzinessException("不能为已离职教师分配学生");
        }
        //学生姓名不能重复 这个业务逻辑错误没有处理，所以运行单元测试会通不过
        //这个在单元测试中可以体现也可以不体现 但是集成测试中一定要体现
        await Set.UpdateAsync(request.Id, o => 
        {
            o.Name = request.Name;
            o.TeacherID = request.TeacherID;
        });
    }

    #region 其它业务，未做示例
    public async Task<AggrResult> AggrAsync(AggrRequest request)
    {
        var data = await PageAsync(request);
        var result = new AggrResult
        {
            Total = data.Total,
            Data = data.Data,
            Teachers = await _teacherService.ListAsync()
        };
        return result;
    }

    public Task<PageResult<StudentItem>> PageAsync(PageRequest request)
    {
        return ReadOnlySet
               .ToPageAsync(request, o => new StudentItem()
               {
                   ID = o.Id,
                   Name = o.Name,
                   获奖总数 = o.Records.Count,
                       //todo 如何实现多个数据库的实体之间的关联
                       Teacher = o.Teacher == null ? string.Empty : o.Teacher.Name
               });
    }


    public Task DelAsync(Guid id)
    {
        return Set.DeleteAsync(id);
    }

    public Task<StudentInfo> InfoAsync(Guid id)
    {
        return ReadOnlySet
            .Where(o => o.Id == id)
            .SingleAsync(o => new StudentInfo()
            {
                IDCard = o.IDCard,
                Image = o.Image,
                Name = o.Name,
                Phone = o.Phone,
                Teacher = o.Teacher == null ? "" : o.Teacher.Name,
                ID = o.Id
            });
    }
    #endregion 其它业务，未做示例
}