using System;
using System.Collections.Generic;
using System.Text;

namespace Nice.Entities
{
    /// <summary>
    /// 标记可以逻辑删除的数据模型
    /// </summary>
    /// <remarks>
    /// [不推荐]统一封装
    /// </remarks>
    public interface ISoftDeletable
    {
        /// <summary>
        /// 标记实体是否已经被删除
        /// </summary>
        bool IsDeleted { get; set; }
    }
}