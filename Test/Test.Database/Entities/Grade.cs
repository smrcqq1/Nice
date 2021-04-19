using System;
using System.Collections.Generic;
using System.Text;

namespace Test.Database.Entities
{
    public class Grade : Nice.Entities.IEntitybase
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
}
