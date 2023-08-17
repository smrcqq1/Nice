using Microsoft.AspNetCore.Http;

namespace Nice.WebAPI
{
    /// <summary>
    /// 提供用户信息
    /// </summary>
    public interface IUserInfo
    {
        Guid? UserId { get; set; }
    }

    internal class UserInfo
    {
        public UserInfo(IHttpContextAccessor httpContextAccessor)
        {
            //httpContext = httpContextAccessor.HttpContext.User;
        }
    }
}