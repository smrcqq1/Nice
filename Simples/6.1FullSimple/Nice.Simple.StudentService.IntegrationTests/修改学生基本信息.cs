namespace Nice.Simple.StudentService.IntegrationTests;
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    [TestClass]
public class 修改学生基本信息 : 学生接口测试环境
{
    [TestMethod]
    public void 成功()
    {
        var service = StudentService;

        //先记录修改前数据
        var addedId = service.AddAsync(new Contracts.DTO.AddStudentRequest()
        {
            Name = "学生修改前名称",
            TeacherID = 在职教师ID
        }).Result;
        Assert.IsNotNull(addedId);
        var beforeEdit = service.InfoAsync(addedId).Result;

        //执行修改
        var editRequest = new Contracts.DTO.Students.EditStudentRequest()
        {
            Id = addedId,
            Name = "学生修改后名称",
            TeacherID = 在职教师ID
        };
        service.PostAsync(editRequest).Wait();
        var student = service.InfoAsync(addedId).Result;
        Assert.IsNotNull(student);

        //检查目标字段是否被正确修改
        Assert.AreEqual(editRequest.Id, student.ID);
        Assert.AreEqual(editRequest.Name, student.Name);

        //检查其它字段是否被错误的修改掉，这步可以酌情不写
        Assert.AreEqual(beforeEdit.IDCard, student.IDCard);
        Assert.AreEqual(beforeEdit.Image, student.Image);
        Assert.AreEqual(beforeEdit.Phone, student.Phone);
    }
    [TestMethod]
    //此例专门用于演示业务逻辑未处理
    //它肯定是无法通过单元测试的,故意用来演示的
    public void 姓名不能重复()
    {
        var service = StudentService;

        //先记录修改前数据
        var student1 = new AddStudentRequest()
        {
            Name = "学生1名称",
            TeacherID = 在职教师ID
        };
        var addResult = service.AddAsync(student1).Result;
        var student2 = new AddStudentRequest()
        {
            Name = "学生2名称",
            TeacherID = 在职教师ID
        };
        var student2ID = service.AddAsync(student2).Result;

        //执行修改
        //在业务逻辑中没有处理学生姓名不能重复这个逻辑 所以这里测试用例无法通过
        ThrowBuzinessException("学生姓名不能重复",() => service.PostAsync(new Contracts.DTO.Students.EditStudentRequest()
        {
            Id = student2ID,//修改第2个学生姓名与第1个相同
            Name = student1.Name,
            TeacherID = 在职教师ID
        }));
    }
    [TestMethod]
    public void 不能为已离职教师分配学生()
    {
        var service = StudentService;
        var request = new AddStudentRequest()
        {
            Name = "学生1",
            TeacherID = 在职教师ID
        };
        var studentID = service.AddAsync(request).Result;

        ThrowBuzinessException("不能为已离职教师分配学生",() => StudentService.PostAsync(new Contracts.DTO.Students.EditStudentRequest()
        {
            Id = studentID,
            Name = "修改后名称",
            TeacherID = 已离职教师ID
        }));
    }
}