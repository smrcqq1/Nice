namespace Nice
{
    /// <summary>
    /// 拥有名称和ID两个字段实体基类
    /// </summary>
    /// <remarks>
    /// [不推荐]统一封装
    /// 标记了此接口的任意实体可以简单的转为Items供下拉框使用
    /// </remarks>
    public interface INamedItem:IIDItem
    {
        /// <summary>
        /// 名称,一般用于显示
        /// </summary>
        string Name { get; set; }
    }
}
