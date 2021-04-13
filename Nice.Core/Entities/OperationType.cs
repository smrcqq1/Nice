using System;
using System.Collections.Generic;
using System.Text;

namespace Nice.Entities
{
    /// <summary>
    /// 操作类型枚举
    /// </summary>
    public enum OperationType
    {
        Unknown = 0,
        Add = 1,
        Delete = 2,
        Edit = 3,
        Query = 4,
        Lock = 5,
        UnLock = 6
    }
}
