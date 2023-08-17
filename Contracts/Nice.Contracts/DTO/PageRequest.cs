namespace Nice.DTO
{
    /// <summary>
    /// 分页查询条件的统一封装
    /// </summary>
    public class PageRequest { 
        /// <summary>
        /// 分页大小
        /// </summary>
        public int Size { get; set; }
        /// <summary>
        /// 分页页面
        /// </summary>
        public int Index { get; set; }
        /// <summary>
        /// 默认的
        /// </summary>
        public static PageRequest Default { get; } = new PageRequest() { Size = 15,Index = 1};
    }
}
