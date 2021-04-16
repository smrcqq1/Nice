using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Database.Entities;

namespace ORMTest
{
    [TestClass]
    public class ReadWriteTest : TestBase
    {
        public Nice.ORM.IReadWriteQueryable<Teacher> Teachers => Orm.Set<Teacher>();

        public Nice.ORM.IReadWriteQueryable<Student> Students => Orm.Set<Student>();

        [TestMethod]
        public void Add_DeleteAsync()
        {
            //var list = Teachers.ToListAsync(o=>o).Result;
            //var teacher = new Teacher();
            //int index = 1;
            //do
            //{
            //    teacher.Name = "教师" + index;
            //    index++;
            //}
            //while (list.Any(o=>o.Name == teacher.Name));
            //var res = Teachers.AddAsync(teacher).Result;
            //Assert.IsTrue(res);

            //var query = Teachers.Where(o=>o.Name == teacher.Name).SingleOrDefaultAsync(o=>o).Result;
            //Assert.IsNotNull(query);

            //var delete = Teachers.DeleteAsync(query.ID).Result;
            //Assert.IsTrue(delete);

            //var deletedTeacher = Teachers.Where(o => o.Name == teacher.Name).SingleOrDefaultAsync(o=>o).Result;
            //Assert.IsNull(deletedTeacher);
        }
    }
}