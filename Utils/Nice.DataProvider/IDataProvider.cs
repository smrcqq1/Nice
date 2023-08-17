namespace Nice
{
    /// <summary>
    /// 数据提供类封装
    /// </summary>
    public interface IDataProvider
    {
        /// <summary>
        /// 获取一个可读可写的数据集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IReadWriteQueryable<T> Set<T>() where T : class, IDataTable, new();
        /// <summary>
        /// 获取一个只读的数据集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IReadOnlyQueryable<T> ReadOnlySet<T>() where T : class, IDataTable, new();
        /// <summary>
        /// 批量新建数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="newT"></param>
        /// <returns></returns>
        Task AddAsync<T>(T[] newT) where T : class, IDataTable, new();
        /// <summary>
        /// 批量新建数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="newT"></param>
        /// <returns></returns>
        Task AddAsync<T>(T newT) where T : class, IDataTable, new();
        /// <summary>
        /// 删除指定的一条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteAsync<T>(Guid id) where T : class, IDataTable, new();
        /// <summary>
        /// 删除指定的所有数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task DeleteAsync<T>(Guid[] ids) where T : class, IDataTable, new();
        /// <summary>
        /// 更新指定的一条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        Task UpdateAsync<T>(Guid id,Action<T> expression) where T : class, IDataTable, new();
        /// <summary>
        /// 更新指定的多条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ids"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        Task UpdateAsync<T>(Guid[] ids, Action<T> expression) where T : class, IDataTable, new();
    }
}