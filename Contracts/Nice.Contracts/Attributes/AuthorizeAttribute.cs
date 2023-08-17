namespace Nice.Attributes
{
    /// <summary>
    /// 标记需要登录才能调用的接口
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute
    {
    }
}