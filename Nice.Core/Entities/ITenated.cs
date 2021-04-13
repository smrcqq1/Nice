using System;
using System.Collections.Generic;
using System.Text;

namespace Nice.Entities
{
    /// <summary>
    /// 多租户模型的标记
    /// </summary>
    /// <remarks>
    /// [推荐]统一封装
    /// </remarks>
    public interface ITenated
    {
        /// <summary>
        /// 租户ID
        /// </summary>
        int TenatID { get; set; }
    }
}
