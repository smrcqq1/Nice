using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Nice
{
    /// <summary>
    /// 远程调用统一封装
    /// </summary>
    public interface IRPC
    {
        Task<TResult> Post<TResult>();
    }
}
