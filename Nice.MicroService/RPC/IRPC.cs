using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Nice
{
    /// <summary>
    /// 远程调用统一封装
    /// </summary>
    public interface IRPC: IInterceptor
    {
        Task<string> Get(string url,Dictionary<string,string> headers = null);
        Task<string> Post(string url,object data, Dictionary<string, string> headers = null);
    }
}
