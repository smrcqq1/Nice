namespace Nice
{
    /// <summary>
    /// 标记接口可能出现的业务错误
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    [AttributeUsage(AttributeTargets.Method,AllowMultiple = true,Inherited = true)]
    public class BuzinessErrorAttribute : Attribute
    {
        /// <summary>
        /// 标记接口可能出现的业务错误
        /// </summary>
        /// <param name="message">错误提示</param>
        /// <remarks>
        /// todo: 错误提示支持模板{{}}占位{version}
        /// </remarks>
        public BuzinessErrorAttribute(string message)
        {
            Message = message;
        }
        /// <summary>
        /// 业务逻辑错误的提示文本
        /// </summary>
        public string Message;
    }
}