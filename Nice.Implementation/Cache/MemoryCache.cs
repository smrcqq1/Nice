using System;
using System.Threading.Tasks;

namespace Nice.Cache
{
    /// <summary>
    /// 本地内存缓存
    /// </summary>
    public class MemoryCache : ICache
    {
        public Task<T> Get<T>(string key)
        {
            throw new NotImplementedException();
        }

        public string Get(string key)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Set<T>(string key, T data, int expireTime = -1)
        {
            throw new NotImplementedException();
        }
    }
}
