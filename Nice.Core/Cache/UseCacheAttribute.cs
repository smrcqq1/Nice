#region using
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
#endregion using
namespace Nice.Cache
{
    /// <summary>
    /// 需要缓存数据的接口请打上此标记
    /// </summary>
    /// <remarks>
    /// 1.仅支持严格按照键值对的提参方式的get请求
    /// 2.默认以完整路径(即url去掉站点地址的部分)作为key
    /// 3.可以注入不同的Cache实现,来使用如redis,静态化,内存缓存等等,并可以随意切换
    /// </remarks>
    public class UseCacheAttribute : Attribute, IActionFilter
    {
        /// <summary>
        /// 经过指定秒数后失效
        /// </summary>
        /// <param name="expireSeconds"></param>
        /// <remarks>
        /// 0:默认,以次日4点钟为所有缓存失效时间
        /// -1:永远不失效
        /// 其它:以秒为单位
        /// </remarks>
        public UseCacheAttribute(int expireSeconds = 0)
        {

        }
        /// <summary>
        /// 指定时间失效
        /// </summary>
        /// <param name="expireTime"></param>
        /// <remarks>
        /// null:默认,以次日4点钟为所有缓存失效时间
        /// </remarks>
        public UseCacheAttribute(DateTime? expireTime = null)
        {

        }

        public ICache Cache { get; set; }
        /// <summary>
        /// 执行成功后,将改数据放入缓存
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuted(ActionExecutedContext context)
        {
            Cache.Set(GetKey(context.HttpContext), context.Result);
        }
        /// <summary>
        /// 获取缓存的key,默认以完整url去掉ip地址为key
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        string GetKey(HttpContext httpContext)
        {
            var key = $"{httpContext.Request.Path}?{httpContext.Request.QueryString}";
            return key;
        }
        /// <summary>
        /// 方法执行前,
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            //只有get请求能被缓存
            if (context.HttpContext.Request.Method != "GET") 
            {
                return;
            }
            var res = Cache.Get<IActionResult>(GetKey(context.HttpContext));
            if (res != null)
            {
                context.Result = res.GetAwaiter().GetResult();
                //context.Result = new ContentResult() {
                //    Content = res,
                //    StatusCode = 200,
                //    ContentType = "Application/json"
                //};
            }
        }
    }
}