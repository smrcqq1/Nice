using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nice;
using Nice.RPC;
using System.Threading.Tasks;

namespace RPCTest
{
    [TestClass]
    public class UnitTest1:Nice.UnitTestBase
    {
        protected override IServiceCollection Config(IServiceCollection services)
        {
            //将其它微服务的Service注入进来,像本地Service一样使用
            return services.UseRPC<HttpRPC>(typeof(UnitTest1).Assembly);
        }
        [TestMethod]
        public void TestMethod1()
        {
            var t = GetService<ITestInterface>();
            var p = "1111111";
            var result = t.Test(p).Result;
            Assert.AreEqual(result,p);
        }
    }
    public interface ITestInterface
    {
        Task<Nice.DTO.NamedItem> Test(string pppp);
    }
}
