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
    public interface IRPC: IAsyncInterceptor, IInterceptor
    {
        string BaseURL { get; }

        /// <summary>
        /// 调用远程接口并获取反序列化的结果
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="invocation"></param>
        Task<T> Send<T>(IInvocation invocation);
        /// <summary>
        /// 调用远程接口并且木有返回值(从HttpCode判断是否成功)
        /// </summary>
        /// <param name="invocation"></param>
        /// <returns></returns>
        Task Send(IInvocation invocation);
    }
}
