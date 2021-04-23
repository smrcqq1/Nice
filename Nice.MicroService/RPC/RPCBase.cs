using Castle.DynamicProxy;
using System;
using System.Threading.Tasks;

namespace Nice.RPC
{
    /// <summary>
    /// 各种RPC方案的基类
    /// </summary>
    public abstract class RPCBase:IRPC
    {
        public abstract string BaseURL { get; }

        public void Intercept(IInvocation invocation)
        {
            this.ToInterceptor().Intercept(invocation);
        }

        public void InterceptSynchronous(IInvocation invocation)
        {
            throw new NotImplementedException();
        }

        public void InterceptAsynchronous(IInvocation invocation)
        {
            invocation.ReturnValue = Send(invocation);
        }
        public void InterceptAsynchronous<TResult>(IInvocation invocation)
        {
            invocation.ReturnValue = Send<TResult>(invocation);
        }
        public abstract Task<T> Send<T>(IInvocation invocation);
        public abstract Task Send(IInvocation invocation);
    }
}