#region using
using Nice.Simple.Contracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion using
namespace Nice.Simple.DataModel
{
    /// <summary>
    /// 教师表
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    public class Teacher: IDataTable
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public TeacherStatus? Status { get; set; }
        public virtual List<Student> Students { get; set; }
        public DateTime CreateTime { get; set; }
    }
}