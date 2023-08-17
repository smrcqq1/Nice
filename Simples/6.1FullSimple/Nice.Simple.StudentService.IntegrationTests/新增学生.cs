namespace Nice.Simple.StudentService.IntegrationTests;

[TestClass]
public class 新增学生 : 学生接口测试环境
{
    /// <summary>
    /// 开发这边只需要这一条测试就可以了
    /// 因为开发这边没有数据库，webapi等环境，所以在开发阶段必须通过这种方式来调试业务，从而使得开发必须要写单元测试，强制测试驱动开发
    /// 只要通过了这个测试用例，即可认为任务已完成，接下来的问题都按BUG处理
    /// 每个用例都在单独的全新的上下文中执行，随意增删改数据绝对不会影响其它用例
    /// </summary>
    [TestMethod]
    public void 成功()
    {
        var service = StudentService;
        var request = new AddStudentRequest()
        {
            Name = "学生1",
            TeacherID = 在职教师ID
        };
        var result = service.AddAsync(request).Result;
        Assert.IsNotNull(result);

        var info = service.PageAsync(new DTO.PageRequest()
        {
            Size = 9999,Index = 0
        }).Result;
    }
    /// <summary>
    /// 以下这些测试都针对业务逻辑进行，开发可以写也可以不写，但是负责集成测试的人必须要写，而且集成测试的时候不需要Mock数据，所以不会很复杂
    /// 接口上定义了多少个业务逻辑错误，就至少应该有多少个用例
    /// 如果你觉得测试用例没有覆盖到，一定要报告负责人，经过讨论后，必须先更新契约层再更新测试用例
    /// 原则上每发现和处理1个bug都应该有一个对应的测试用例
    /// </summary>
    [TestMethod]
    public void 姓名不能重复()
    {
        var service = StudentService;
        var request = new AddStudentRequest()
        {
            Name = "学生1",
            TeacherID = 在职教师ID
        };
        var result = service.AddAsync(request).Result;
        Assert.IsNotNull(result);
        //再次增加一个同名学生，应该抛出Message=学生姓名不能重复的BuzinessException
        //业务逻辑错误都是通过抛出BuzinessException来处理的，这里做了一个简单封装
        ThrowBuzinessException("学生姓名不能重复",() => service.AddAsync(request));
    }
    [TestMethod]
    public void 不能为已离职教师分配学生()
    {
        var request = new AddStudentRequest()
        {
            Name = "学生1",
            TeacherID = 已离职教师ID
        };
        ThrowBuzinessException("不能为已离职教师分配学生",() => StudentService.AddAsync(request));
    }
}