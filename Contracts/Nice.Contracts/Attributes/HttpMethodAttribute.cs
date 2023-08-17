namespace Nice.Attributes
{
    /// <summary>
    /// 支持的HttpMethod枚举
    /// </summary>
    public enum HttpMethodType
    {
        /// <summary>
        /// HttpGet
        /// </summary>
        HttpGet,
        /// <summary>
        /// HttpPost
        /// </summary>
        HttpPost,
        /// <summary>
        /// HttpPut
        /// </summary>
        HttpPut,
        /// <summary>
        /// HttpDelete
        /// </summary>
        HttpDelete,
    }
    /// <summary>
    /// 标记接口的HttpMethod类型
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    [AttributeUsage(AttributeTargets.Method)]
    public class HttpMethodAttribute : Attribute
    {
        /// <summary>
        /// 获取配置的HttpMethod
        /// </summary>
        public HttpMethodType HttpMethodType { get;private set; }
        /// <summary>
        /// 指定方法的HttpMethod
        /// </summary>
        /// <param name="httpMethodType"></param>
        public HttpMethodAttribute(HttpMethodType httpMethodType)
        {
            HttpMethodType = httpMethodType;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    public class HttpGetAttribute : HttpMethodAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="template"></param>
        public HttpGetAttribute():base(HttpMethodType.HttpGet)
        {

        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class HttpPostAttribute : HttpMethodAttribute
    {/// <summary>
    /// 
    /// </summary>
    /// <param name="template"></param>
        public HttpPostAttribute() : base(HttpMethodType.HttpPost)
        {

        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    public class HttpPutAttribute : HttpMethodAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="template"></param>
        public HttpPutAttribute() : base(HttpMethodType.HttpPut)
        {

        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    public class HttpDeleteAttribute : HttpMethodAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="template"></param>
        public HttpDeleteAttribute() : base(HttpMethodType.HttpDelete)
        {

        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Method)]
    public class RouteAttribute : Attribute
    {
        /// <summary>
        /// 使用指定的名称来路由此接口或方法
        /// </summary>
        /// <param name="template">路由模板</param>
        /// <remarks>
        /// todo: 路由模板支持{version}
        /// </remarks>
        public RouteAttribute(string template = "")
        {
            Template = template;
        }
        /// <summary>
        /// 路由设置
        /// </summary>
        public string? Template { get; private set; }
    }
}
