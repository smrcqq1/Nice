using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Test.Database.Entities;

namespace ORMTest
{
    [TestClass]
    public class ReadOnlyTest : TestBase
    {
        public Nice.ORM.IReadOnlyQueryable<Teacher> Teachers => Orm.ReadOnlySet<Teacher>();

        public Nice.ORM.IReadOnlyQueryable<Student> Students => Orm.ReadOnlySet<Student>();

        [TestMethod]
        public void CountAsync()
        {
            int cnt = Teachers.CountAsync().Result;
            Assert.IsTrue(cnt > 0);

            cnt = Teachers.Where(o => o.Name.Contains("不存在的教师")).CountAsync().Result;
            Assert.IsFalse(cnt > 0);

            cnt = Teachers.Where(o => o.Name.Contains("教师")).CountAsync().Result;
            Assert.IsTrue(cnt > 0);
        }
        [TestMethod]
        public void AnyAsync()
        {
            var cnt = Teachers.AnyAsync().Result;
            Assert.IsTrue(cnt);

            cnt = Teachers.Where(o => o.Name.Contains("不存在的教师")).AnyAsync().Result;
            Assert.IsFalse(cnt);

            cnt = Teachers.Where(o => o.Name.Contains("教师")).AnyAsync().Result;
            Assert.IsTrue(cnt);

        }
        [TestMethod]
        public void SingleAsync()
        {
            var list = Teachers.ToListAsync(o => o).Result;
            var res = Teachers.SingleAsync(list[0].ID, o => o).Result;
            Assert.IsNotNull(res);
            try
            {
                res = Teachers.Where(o => o.Name.Contains("不存在的教师")).SingleAsync(o => o).Result;
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.InnerException.Message == "404");
            }
        }
    }
}
