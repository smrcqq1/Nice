using System;
using System.Collections.Generic;
using System.Text;

namespace Nice.Simple.DataModel
{
    public class 获奖记录 : Nice.IDataTable
    {
        public Guid Id { get; set; }
        public DateTime CreateTime { get; set; } = new DateTime();

        public virtual Student Student { get; set; }
        public virtual Guid StudentID { get; set; }
    }
}
