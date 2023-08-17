namespace Nice.Simple.Contracts.DTO
{
    /// <summary>
    /// 学生详细信息，用于详情页
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    public class StudentInfo:StudentItem
    {
        /// <summary>
        /// 身份证号码
        /// </summary>
        public string? IDCard { get; set; }
        /// <summary>
        /// 照片
        /// </summary>
        public string? Image { get; set; }
        /// <summary>
        /// 电话号码
        /// </summary>
        public string? Phone { get; set; }
    }
}
