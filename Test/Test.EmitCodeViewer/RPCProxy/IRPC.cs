using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nice
{
    /// <summary>
    /// 远程调用统一封装
    /// </summary>
    public interface IRPC
    {
        string BaseURL { get; }
        Task<T> Get<T>(string url,Dictionary<string,string> headers = null);
        Task<T> Post<T>(string url,object data, Dictionary<string, string> headers = null);
    }
}
