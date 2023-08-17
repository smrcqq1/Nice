namespace Nice.Simple.Contracts.DTO
{
    /// <summary>
    /// 学生管理页面的初始化聚合结果
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    public class AggrResult:PageResult<StudentItem>
    {
        /// <summary>
        /// 教师下拉框的数据源
        /// </summary>
        public List<Teacher>? Teachers { get; set; }
    }
}
