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
    public class HttpRPC : RPCBase
    {
        public override string BaseURL => "";

        public override Task<TResult> Send<TResult>(IInvocation invocation)
        {
            #region 如果是get请求,组装url和参数
            var index = 0;
            var sb = new StringBuilder(BaseURL);
            sb.Append('?');
            foreach (var p in invocation.Method.GetParameters())
            {
                var value = invocation.GetArgumentValue(index);
                sb.Append(p.Name);
                sb.Append('=');
                sb.Append(value);
                sb.Append('&');
                index++;
            }
            #endregion 如果是get请求,组装url和参数
            var url = sb.ToString();

            #region 如果是POST请求,组装body
            #endregion 如果是POST请求,组装body

            #region 调用远程真正的方法,获取回复
            var res = "[{\"ID\":\"111\",\"Name\":\"测试\"}]";
            #endregion 调用远程真正的方法,获取回复

            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<TResult>(res);
            return Task.FromResult(obj);
        }

        public override Task Send(IInvocation invocation)
        {
            return Task.CompletedTask;
        }
    }
}