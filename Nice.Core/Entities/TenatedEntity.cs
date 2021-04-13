using System;
using System.Collections.Generic;
using System.Text;

namespace Nice.Entities
{
    /// <summary>
    /// 多租户系统中的模型基类统一封装
    /// </summary>
    public class TenatedEntity : Entitybase, ITenated
    {
        public int TenatID { get; set; }
    }
}