namespace Nice.WebAPI
{
    public interface INiceControllerBuilder
    {
        /// <summary>
        /// 将T所在Assembly的所有标记了Nice.IAPI的接口和类映射为Controller
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="front">路由前缀</param>
        INiceControllerBuilder AddControllers<T>(string front = "");
        /// <summary>
        /// 使用通用异常处理
        /// </summary>
        /// <returns></returns>
        INiceControllerBuilder AddGlobalExceptionHandler();
        /// <summary>
        /// 设置当参数检查发现错误的时候，如何返回给前端
        /// </summary>
        /// <param name="errorToResult"></param>
        /// <returns></returns>
        INiceControllerBuilder SetModelValidator(Func<string[], object> errorToResult);
        /// <summary>
        /// 使用通用文件处理，包括上传、下载、删除、移动和更新等常用文件操作
        /// </summary>
        /// <returns></returns>
        INiceControllerBuilder AddFileHandler(string front = "");
        /// <summary>
        /// 为接口配置缓存
        /// </summary>
        /// <returns></returns>
        INiceControllerBuilder AddCaching(ICachingBuilder builder);
        /// <summary>
        /// 配置接口返回消息统一规范
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// 可以将nice的返回数据的风格，转换为各公司自己的风格
        /// </remarks>
        INiceControllerBuilder SetResponseSpecification(Func<object,object> messageHandler);
        /// <summary>
        /// 缓存设置，允许配置多种多个缓存
        /// </summary>
        List<ICachingBuilder> CachingBuilders { get; }
    }
}