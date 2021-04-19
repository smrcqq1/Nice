using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nice.RPC
{
    /// <summary>
    /// 基于HTTP的RPC实现
    /// </summary>
    public class HttpRPC : IRPC
    {
        public string BaseURL
        {
            get
            {
                return "";
            }
        }
        public void Intercept(IInvocation invocation)
        {
            var index = 0;
            var sb = new StringBuilder(BaseURL);
            sb.Append("?");
            foreach(var p in invocation.Method.GetParameters())
            {
                var value = invocation.GetArgumentValue(index);
                sb.Append(p.Name);
                sb.Append('=');
                sb.Append(value);
                sb.Append('&');
                index++;
            }
            var res = Get(sb.ToString()).Result;
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject(res, invocation.Method.ReturnType.GenericTypeArguments[0]);
            invocation.ReturnValue = Task.FromResult(obj);
            #warning 这样实现行不通
        }

        public async Task<T> Get<T>(string url, Dictionary<string, string> headers = null)
        {
            var str = await Get(url,headers);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(str);
        }

        public  Task<string> Get(string url, Dictionary<string, string> headers = null)
        {
            return Task.FromResult(Newtonsoft.Json.JsonConvert.SerializeObject(new DTO.NamedItem() { ID = 1,Name = url }));
        }

        public Task<T> Post<T>(string url, object data, Dictionary<string, string> headers = null)
        {
            throw new NotImplementedException();
        }
    }
}