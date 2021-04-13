using Nice.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nice.ORM
{
    /// <summary>
    /// 如果数据需要简单的操作记录,请实现此接口
    /// </summary>
    /// <remarks>
    /// [不推荐]统一封装
    /// </remarks>
    public interface IOperationRecord
    {
        /// <summary>
        /// 简单的操作记录
        /// </summary>
        List<OperationRecord> OperationRecords { get; set; }
    }
    /// <summary>
    /// 如果需要对修改操作进行更详细的操作记录,请实现此接口
    /// </summary>
    /// <remarks>
    /// [不推荐]统一封装
    /// </remarks>
    public interface IModifyRecord
    {
        /// <summary>
        /// 详细的修改记录
        /// </summary>
        /// <remarks>
        /// 主要是修改操作会具体到每个字段的旧值与新值记录
        /// </remarks>
        List<DetailOperationRecord> OperationRecords { get; set; }
    }
}