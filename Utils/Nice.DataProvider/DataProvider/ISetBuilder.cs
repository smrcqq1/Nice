namespace Nice.DataProvider
{
    /// <summary>
    /// 对数据集进行数据提供程序的接口定义
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    public interface ISetBuilder
    {
        /// <summary>
        /// 本builder包含对哪些数据模型
        /// </summary>
        List<Type> Types { get; }
        //ISetBuilder UseEFCore<TDbContext, TProvider>() where TDbContext : Signs.IDbContext where TProvider : IDataProvider;
    }
}