using System;

namespace CachingTest
{
    public class Student : Nice.IDataTable
    {
        public Guid Id { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
