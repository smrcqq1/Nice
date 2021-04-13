using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nice.RPC
{
    public class HttpRPC : IRPC,IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            var str = Get("").Result;
            var res = str.FromJson(invocation.Method.ReturnType);
            invocation.ReturnValue = Task.FromResult(res);
        }
        public Task<string> Get(string url, Dictionary<string, string> headers = null)
        {
            return Task.FromResult(Newtonsoft.Json.JsonConvert.SerializeObject(new DTO.NamedItem() { ID = 1,Name = "学生1"}));
        }

        public Task<TResult> Get<TResult>(string url, Dictionary<string, string> headers = null)
        {
            throw new NotImplementedException();
        }

        public Task<string> Post(string url, object data, Dictionary<string, string> headers = null)
        {
            throw new NotImplementedException();
        }

        public Task<TResult> Post<TResult>(string url, object data, Dictionary<string, string> headers = null)
        {
            throw new NotImplementedException();
        }
    }
}
