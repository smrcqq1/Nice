using Nice.ParameterCheck;
namespace Tests
{
    [TestClass]
    public class ValidatExtentionsTests : Nice.Tests.TestBase
    {
        [TestMethod]
        public void 数组测试()
        {
            var item = new 测试对象();
            var targets = new List<测试对象>();
            ThrowParameterErrorException("数组不能为空", () => {
                var c = new CheckArray<测试对象>(targets);
                c.NotNull("数组不能为空")
             .Throw();
            });
        }
    }
    class 测试对象
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }

        public DateTime BirthDay { get; set; }
        public DateTime? DieDay { get; set; }

        public string[] NickNames { get; set; }

        public 测试对象[] Children { get; set; }
    }
}