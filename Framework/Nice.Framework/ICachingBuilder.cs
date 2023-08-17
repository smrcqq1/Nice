using System.Linq.Expressions;

namespace Nice
{
    /// <summary>
    /// 缓存配置器接口定义
    /// </summary>
    public interface ICachingBuilder
    {
        /// <summary>
        /// 获取缓存提供程序
        /// </summary>
        List<ICachingProvider> CachingProviders { get; }
        /// <summary>
        /// 获取所有的缓存配置
        /// </summary>
        List<CachingSetting> Settings { get; }
        /// <summary>
        /// 当前正在进行配置的缓存
        /// </summary>
        CachingSetting Current { get; set; }
    }
    /// <summary>
    /// 缓存配置
    /// </summary>
    public class CachingSetting
    {
        /// <summary>
        /// 待缓存接口
        /// </summary>
        public List<Expression> ActionExpression { get; set; } = new List<Expression>();
        /// <summary>
        /// 更新接口，如过有，则当此方法被调用的时候会自动更新缓存
        /// </summary>
        public List<Expression> UpdateExpression { get;private set; } = new List<Expression>();
        /// <summary>
        /// 过期时间，单位：秒
        /// </summary>
        public int? ExpireSecond { get; set; }
    }
}