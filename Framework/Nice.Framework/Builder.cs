using Microsoft.Extensions.Hosting;

namespace Nice
{
    /// <summary>
    /// Nice架构的框架配置器
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    public class Builder
    {
        internal IHostBuilder HostBuilder { get; private set; }
        /// <summary>
        /// 配置使用的HostBuilder
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public Builder(IHostBuilder builder)
        {
            HostBuilder = builder;
        }
    }
}