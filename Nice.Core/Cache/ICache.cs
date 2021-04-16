using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Nice
{
    /// <summary>
    /// 缓存统一封装,可以根据需要涵盖redis,静态化等
    /// </summary>
    public interface ICache
    {
        /// <summary>
        /// 添加到缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">指定key</param>
        /// <param name="data">指定数据</param>
        /// <param name="expireTime">过期时间</param>
        /// <returns></returns>
        Task<bool> Set<T>(string key,T data,int expireTime = -1);
        /// <summary>
        /// 获取指定key的缓存数据,并转换为指定类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<T> Get<T>(string key);
        /// <summary>
        /// 获取指定key的缓存数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string Get(string key);
    }
}