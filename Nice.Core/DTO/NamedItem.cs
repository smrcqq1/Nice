namespace Nice.DTO
{
    /// <summary>
    /// 有名称和ID两个字段的类型
    /// </summary>
    public class NamedItem: IDItem,INamedItem
    {
        /// <summary>
        /// 名称,一般用于显示
        /// </summary>
        public string Name { get; set; }
    }
}
