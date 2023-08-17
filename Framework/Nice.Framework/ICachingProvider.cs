namespace Nice;
/// <summary>
/// 缓存提供方接口定义
/// </summary>
public interface ICachingProvider
{
    /// <summary>
    /// 读取缓存
    /// </summary>
    /// <param name="cacheKey">键</param>
    /// <returns></returns>
    Task<T?> Read<T>(string cacheKey) where T : class;
    /// <summary>
    /// 写入缓存
    /// </summary>
    /// <param name="value">对象数据</param>
    /// <param name="cacheKey">键</param>
    Task Write<T>(string cacheKey, T value) where T : class;
    /// <summary>
    /// 写入缓存
    /// </summary>
    /// <param name="value">对象数据</param>
    /// <param name="cacheKey">键</param>
    /// <param name="expireTime">到期时间</param>
    Task Write<T>(string cacheKey, T value, DateTime? expireTime = null) where T : class;
    /// <summary>
    /// 写入缓存
    /// </summary>
    /// <param name="value">对象数据</param>
    /// <param name="cacheKey">键</param>
    /// <param name="expireTime">到期时间</param>
    Task Write<T>(string cacheKey, T value, TimeSpan? expireTime = null) where T : class;
    /// <summary>
    /// 移除指定数据缓存
    /// </summary>
    /// <param name="cacheKey">键</param>
    Task Remove(string cacheKey);
    /// <summary>
    /// 查询指定key的缓存是否存在
    /// </summary>
    /// <param name="cacheKey">键</param>
    Task<bool> Existed(string cacheKey);
    /// <summary>
    /// 移除全部缓存
    /// </summary>
    Task Clear();
}