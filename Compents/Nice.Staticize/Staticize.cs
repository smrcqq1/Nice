#region using
using Nice.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#endregion using
namespace Nice.Cache
{
    /// <summary>
    /// 缓存的静态化实现
    /// </summary>
    public class Staticize : IStaticize
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