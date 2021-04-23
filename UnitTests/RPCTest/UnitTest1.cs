using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nice;
using Nice.RPC;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RPCTest
{
    [TestClass]
    public class UnitTest1:Nice.UnitTestBase
    {
        protected override IServiceCollection Config(IServiceCollection services)
        {
            //将其它微服务的Service注入进来,然后像本地Service一样使用
            return services.UseRPC<ITestInterface>(new HttpRPC());
        }
        [TestMethod]
        public void TestMethod1()
        {
            //var example = new Nice.Example();
            //example.Test().GetAwaiter().GetResult();
            //注意 ITestInterface 是一个远程接口,并且在本地没有实现,但可以像本地Service一样使用
            var t = GetService<ITestInterface>();
            var result = t.Get().Result;
        }
    }
    public interface ITestInterface
    {
        Task<List<Nice.DTO.NamedItem>> Get();
        Task<Nice.DTO.NamedItem> Get(int id);
        Task<bool> Edit(Nice.DTO.NamedItem request);
    }
}