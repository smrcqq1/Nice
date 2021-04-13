using System.Collections.Generic;

namespace Nice.Entities
{
    /// <summary>
    /// 标记无限层级树形结构的实体模型
    /// </summary>
    /// <remarks>
    /// [不推荐]统一封装
    /// </remarks>
    public interface  ITreeable<TSource> where TSource : class,IIDItem
    {
        /// <summary>
        /// 父级ID
        /// </summary>
        int ParentID { get; set; }
        /// <summary>
        /// 父级
        /// </summary>
        TSource Parent { get; set; }
        /// <summary>
        /// 所有子节点
        /// </summary>
        List<TSource> Children { get; set; }
    }
}