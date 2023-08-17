namespace Nice.DTO
{
    /// <summary>
    /// 分页查询的结果统一封装
    /// </summary>
    public class PageResult<T>
    {
        /// <summary>
        /// 数据总条数
        /// </summary>
        public int Total { get; set; }
        /// <summary>
        /// 数据
        /// </summary>
        public List<T>? Data { get; set; }
    }
}
