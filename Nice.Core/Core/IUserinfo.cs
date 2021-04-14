namespace Nice
{
    /// <summary>
    /// 普通用户信息统一接口
    /// </summary>
    public interface IUserinfo:IIDItem
    {

    }
    /// <summary>
    /// 启用了多租户的系统的用户信息统一接口
    /// </summary>
    public interface ITenantProvider : IUserinfo,Entities.ITenated
    {

    }
}