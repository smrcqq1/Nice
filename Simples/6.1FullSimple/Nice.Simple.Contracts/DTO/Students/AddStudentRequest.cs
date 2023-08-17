namespace Nice.Simple.Contracts.DTO
{
    /// <summary>
    /// 新增学生需要的数据
    /// </summary>
    /// <remarks>
    /// 1.网络请求传递的实体，一定要打上Nice.Signs.IDTO标记
    /// </remarks>
    public class AddStudentRequest
    {
        /// <summary>
        /// 学生姓名
        /// </summary>
//todo  【紧急】自动根据注解生成验证代码未实现
//todo  【紧急】自动根据注解生成集成测试用例未实现
        public string Name { get; set; }
        /// <summary>
        /// 该学生的班主任
        /// </summary>
        public Guid TeacherID { get; set; }
    }
}
