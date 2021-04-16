using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nice
{
    public class Example
    {
        public async Task Test()
        {
            var dynamicProxyType = new EmitExtentionsX().CreateProxy<XXXProxy>();

            var provider = new HttpProxyProvider();
            var dynamicProxy = Activator.CreateInstance(dynamicProxyType, provider) as XXXProxy;

            var val = await dynamicProxy.Method1("张三", "打瞌睡");
        }
    }

    public interface XXXProxy
    {
        Task<string> Method1(string arg1, string arg2);
    }

    public class HttpProxyProvider : IProxyProvider
    {
        public Task<object> Invoke(string methodName, IEnumerable<KeyValuePair<string, object>> args)
        {
            //真正的处理逻辑

            throw new NotImplementedException();
        }
    }
}