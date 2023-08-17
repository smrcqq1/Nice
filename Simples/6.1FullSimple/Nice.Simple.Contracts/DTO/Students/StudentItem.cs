namespace Nice.Simple.Contracts.DTO
{
    /// <summary>
    /// 学生简要信息，用于列表
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    public class StudentItem
    {
        public Guid ID { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 该学生的班主任姓名
        /// </summary>
        public string? Teacher { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int 获奖总数 { get; set; }
    }
}
