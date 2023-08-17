#region using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion using
namespace Nice.Simple.Contracts.Enums
{
    /// <summary>
    /// 教师在职状态
    /// </summary>
    /// <remarks>
    /// 用以模拟业务已离职教师不能分配学生
    /// </remarks>
    public enum TeacherStatus
    {
        在职 = 0,
        已离职 = 1
    }
}
