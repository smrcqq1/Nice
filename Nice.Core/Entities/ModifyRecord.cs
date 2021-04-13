using System.Collections.Generic;

namespace Nice.Entities
{
    /// <summary>
    /// 更详尽的修改记录
    /// </summary>
    public class DetailOperationRecord : OperationRecord
    {
        /// <summary>
        /// 针对修改进行更详细的记录
        /// </summary>
        public virtual List<ModifyRecord> ModifyRecords { get; set; }
    }
    /// <summary>
    /// 对修改进行更详细记录的通用的操作记录表
    /// </summary>
    /// <remarks>
    /// 通过实现IModifyRecord接口来使用本类
    /// </remarks>
    public class ModifyRecord : Entitybase
    {
        /// <summary>
        /// 操作记录ID
        /// </summary>
        public int OperationRecordID { get; set; }
        /// <summary>
        /// 操作记录
        /// </summary>
        public virtual OperationRecord OperationRecord { get; set; }
        /// <summary>
        /// 如果是修改,保存操作字段名称
        /// </summary>
        public string FieldName { get; set; }
        /// <summary>
        /// 如果是修改,保存旧值
        /// </summary>
        public string OldValue { get; set; }
        /// <summary>
        /// 如果是修改,保存新值
        /// </summary>
        public string NewValue { get; set; }
    }
}