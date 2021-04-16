using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nice
{
    public interface IProxyProvider
    {
        Task<object> Invoke(string methodName, IEnumerable<KeyValuePair<string, object>> args);
    }
}