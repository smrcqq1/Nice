using System;
using System.Threading.Tasks;
namespace Nice.Cache
{
    public class Redis : ICache
    {
        public Task<T> Get<T>(string key)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Set<T>(string key, T data, int expireTime = -1)
        {
            throw new NotImplementedException();
        }
    }
}