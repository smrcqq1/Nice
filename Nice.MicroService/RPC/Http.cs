using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nice.RPC
{
    /// <summary>
    /// 使用普通HTTP作为RPC
    /// </summary>
    public class Http : IRPC
    {
        public Task<TResult> Post<TResult>()
        {
            throw new NotImplementedException();
        }
    }
}