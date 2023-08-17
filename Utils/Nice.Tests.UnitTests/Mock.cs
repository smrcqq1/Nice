namespace Nice.Tests.UnitTests
{
    [TestClass]
    public class MockTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            MockService.Mock<ITestAPI>()
                .Setup(o => o.TestActionAsync("1"))
                .SetReturns("sdf")
                .Setup(o => o.TestAction("2"))
                .SetReturns("”√ªß2");
        }
    }
    public interface ITestAPI
    {
        string TestAction(string name);
        Task<string> TestActionAsync(string name);
    }
}