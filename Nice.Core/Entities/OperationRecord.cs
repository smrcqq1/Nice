using System;
using System.Collections.Generic;

namespace Nice.Entities
{
    /// <summary>
    /// 通用的操作记录表
    /// </summary>
    /// <remarks>
    /// 通过实现IOperationRecord接口来使用本类
    /// </remarks>
    public class OperationRecord:Entitybase
    {
        /// <summary>
        /// 对应的表名称.通过TableName和RecordID确定唯一业务数据
        /// </summary>
        public string TableName { get; set; }
        /// <summary>
        /// 对应的业务数据记录ID
        /// </summary>
        public int RecordID { get; set; }
        /// <summary>
        /// 操作类型
        /// </summary>
        public OperationType OperationType { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime Time { get; set; }
        /// <summary>
        /// 操作者ID
        /// </summary>
        public int OperatorID { get; set; }
        /// <summary>
        /// 操作者名称(冗余)
        /// </summary>
        public string Operator { get; set; }
    }
}