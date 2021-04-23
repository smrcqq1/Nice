using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MongoDBTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Test.MangoDB.Extentions.Test();
        }
    }
}
