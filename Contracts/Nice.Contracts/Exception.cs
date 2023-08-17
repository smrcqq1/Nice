namespace Nice
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    public class Exception:System.Exception
    {
        /// <summary>
        /// 错误码
        /// </summary>
        public int Code { get; set; } = 500;
        /// <summary>
        /// 构造一个异常
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        public Exception(int code,string message):base(message)
        {
            Code = code;
            ErrMsg = new List<string>() { message };
        }
        /// <summary>
        /// 构造一个异常
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        public Exception(int code, IList<string> message) : base("发生错误")
        {
            Code = code;
            ErrMsg = message;
        }
        public IList<string> ErrMsg { get; set; }
    }
    /// <summary>
    /// 用于向客户端返回业务逻辑错误
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    public class BuzinessException : Exception
    {
        /// <summary>
        /// 构造一个业务逻辑异常
        /// </summary>
        /// <param name="message"></param>
        public BuzinessException(string message) : base(500,message)
        {
        }
        /// <summary>
        /// 构造一个业务逻辑异常
        /// </summary>
        /// <param name="message"></param>
        public BuzinessException(IList<string> message) : base(500, message)
        {
        }
    }
    /// <summary>
    /// 用于向客户端返回DEBUG错误
    /// </summary>
    /// <remarks>
    /// 不能显示给用户看，而是在F12调试模式下的NETWORK中查看
    /// 这个主要是为了在生产环境中排查错误
    /// </remarks>
    public class DebugException : Exception
    {
        /// <summary>
        /// 构造一个用于Debug的异常，该异常经过特殊处理，开发人员可以看到详细信息，但是用户只能看到特殊的友好信息
        /// </summary>
        /// <param name="message"></param>
        public DebugException(string message) : base(998, message)
        {
        }
        /// <summary>
        /// 构造一个用于Debug的异常，该异常经过特殊处理，开发人员可以看到详细信息，但是用户只能看到特殊的友好信息
        /// </summary>
        /// <param name="message"></param>
        public DebugException(IList<string> message) : base(998, message)
        {
        }
    }
}
