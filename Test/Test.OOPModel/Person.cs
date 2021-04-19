using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.OOPModel
{
    public class Person
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int SchoolID { get; set; }
        public virtual School School { get; set; }
    }
}
