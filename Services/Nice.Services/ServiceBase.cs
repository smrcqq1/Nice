namespace Nice.Services;
/// <summary>
/// Service层的基类,提供常用封装
/// </summary>
public abstract class ServiceBase : Nice.ParameterCheck.IParameterCheck
{
    public List<string> Errors { get; } = new();
    public ServiceBase(IDataProvider dataProvider)
    {
        DataProvider = dataProvider;
    }
    protected IDataProvider DataProvider { get;private set; }
}
/// <summary>
/// Service层的泛型基类,提供常用封装
/// </summary>
public abstract class ServiceBase<TDataSet> : ServiceBase where TDataSet : class,IDataTable,new()
{
    public ServiceBase(IDataProvider dataProvider) :base(dataProvider)
    {
    }
    /// <summary>
    /// 检查符合查询表达式的数据是否存在,若存在则抛出异常
    /// </summary>
    /// <param name="expression">查询表达式</param>
    /// <param name="message">错误提示文本</param>
    /// <returns></returns>
    /// <exception cref="Nice.BuzinessException"></exception>
    protected async Task IfExistedThenThrow(Expression<Func<TDataSet,bool>> expression,string message)
    {
        if(await Existed(expression))
        {
            throw new Nice.BuzinessException(message);
        }
    }
    /// <summary>
    /// 检查符合查询表达式的数据是否存在
    /// </summary>
    /// <param name="expression">查询表达式</param>
    /// <returns></returns>
    protected Task<bool> Existed(Expression<Func<TDataSet, bool>> expression)
    {
        return Set.Where(expression).AnyAsync();
    }
    /// <summary>
    /// 获取可读可写的数据库对象
    /// </summary>
    protected IReadWriteQueryable<TDataSet> Set { get { return DataProvider.Set<TDataSet>(); } }

    /// <summary>
    /// 获取可读但不可写的数据库对象
    /// </summary>
    protected IReadOnlyQueryable<TDataSet> ReadOnlySet { get { return DataProvider.ReadOnlySet<TDataSet>(); } }
}