using System;
using System.Collections.Generic;

namespace Test.OOPModel
{
    public class School
    {
        public int ID { get; set; }
        public string Name { get; set; }
        //public virtual Person 校长 { get; set; }
        public virtual List<Person> 学生 { get; set; }
    }
}