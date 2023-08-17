namespace Nice.DataProvider
{
    /// <summary>
    /// 只读的数据查询器
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    public interface IReadWriteQueryable<TSource>
    {
        /// <summary>
        /// 对符合查询条件的所有数据进行批量更新
        /// </summary>
        /// <param name="updateExpression"></param>
        /// <returns></returns>
        /// <remarks>
        /// 注意:此操作不会跟踪
        /// </remarks>
        Task BulkUpdateAsync(Action<TSource> updateExpression);
        /// <summary>
        /// 批量删除符合查询条件的所有数据
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// 注意:此删除不会跟踪
        /// </remarks>
        Task BulkDeleteAsync();
        /// <summary>
        /// 开启事务
        /// </summary>
        /// <returns></returns>
        Task Transaction(Func<Task> action);
        /// <summary>
        /// 如果指定字符串不为空,则增加一个查询条件
        /// </summary>
        /// <returns></returns>
        IReadWriteQueryable<TSource> WhereIf(string condition, Expression<Func<TSource, bool>> expression);
        /// <summary>
        /// 如果指定Guid不为空,则增加一个查询条件
        /// </summary>
        /// <returns></returns>
        IReadWriteQueryable<TSource> WhereIf(Guid? condition, Expression<Func<TSource, bool>> expression);
        /// <summary>
        /// 先检查数据是否存在,若存在则抛出异常;不存在则插入指定的新数据
        /// </summary>
        /// <param name="existedWhere"></param>
        /// <param name="message"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        Task<Guid> AddIfNotExistedAsync(Expression<Func<TSource, bool>> existedWhere, string message, Func<TSource> data);
        /// <summary>
        /// GroupJoin
        /// </summary>
        /// <typeparam name="TInner"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="outerKeySelector"></param>
        /// <param name="innerKeySelector"></param>
        /// <param name="resultSelector"></param>
        /// <returns></returns>
        IReadOnlyQueryable<TResult> GroupJoin<TInner, TKey, TResult>(Expression<Func<TSource, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TSource, IEnumerable<TInner>, TResult>> resultSelector) where TInner : class;
        /// <summary>
        /// GroupBy
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        IReadOnlyQueryable<IGrouping<TKey, TSource>> GroupBy<TKey>(Expression<Func<TSource, TKey>> keySelector);
        /// <summary>
        /// InnerJoin
        /// </summary>
        /// <typeparam name="TInner"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="outerKeySelector"></param>
        /// <param name="innerKeySelector"></param>
        /// <param name="resultSelector"></param>
        /// <returns></returns>
        IReadOnlyQueryable<TResult> Join<TInner, TKey, TResult>(Expression<Func<TSource, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TSource, TInner, TResult>> resultSelector) where TInner : class;
        /// <summary>
        /// InnerJoin，默认只能join两个表，并且都以Guid的key相结合
        /// </summary>
        /// <typeparam name="TInner"></typeparam>
        /// <returns></returns>
        IReadOnlyQueryable<JoinResult<TSource, TInner>> Join<TInner>(Expression<Func<TSource, Guid>> outerKeySelector, Expression<Func<TInner, Guid>> innerKeySelector) where TInner : class;
        /// <summary>
        /// InnerJoin
        /// </summary>
        /// <typeparam name="TInner"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="outerKeySelector"></param>
        /// <param name="innerKeySelector"></param>
        /// <param name="resultSelector"></param>
        /// <returns></returns>
        IReadOnlyQueryable<TResult> LeftJoin<TInner, TKey, TResult>(Expression<Func<TSource, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TSource, TInner, TResult>> resultSelector) where TInner : class;
        /// <summary>
        /// 增加一个查询条件
        /// </summary>
        /// <returns></returns>
        IReadWriteQueryable<TSource> Where(Expression<Func<TSource, bool>> expression);
        /// <summary>
        /// 如果指定条件为真,则增加一个查询条件
        /// </summary>
        /// <returns></returns>
        IReadWriteQueryable<TSource> WhereIf(bool condition, Expression<Func<TSource, bool>> expression);
        ///// <summary>
        ///// 设置源数据到结果集的映射关系
        ///// </summary>
        ///// <typeparam name="TResult"></typeparam>
        ///// <param name="expression"></param>
        ///// <returns></returns>
        //IQueryable<TResult> Select<TResult>(Expression<Func<T, TResult>> expression);
        /// <summary>
        /// 查询符合条件的数据条数
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// 慎用：尽量使用AnyAsync
        /// </remarks>
        Task<int> CountAsync();
        /// <summary>
        /// 查询是否存在符合条件的数据
        /// </summary>
        /// <returns></returns>
        Task<bool> AnyAsync();
        /// <summary>
        /// 查询分页数据
        /// </summary>
        /// <returns></returns>
        Task<PageResult<TResult>> ToPageAsync<TResult>(PageRequest request, Expression<Func<TSource, TResult>> selector);
        /// <summary>
        /// 查询多条数据
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// 慎用：不允许使用这个方法来查询单条数据，业务一定要理解透
        /// </remarks>
        Task<List<TResult>> ToListAsync<TResult>(Expression<Func<TSource, TResult>> selector);
        /// <summary>
        /// 查询多条数据
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// 慎用：不允许使用这个方法来查询单条数据，业务一定要理解透
        /// </remarks>
        Task<TResult[]> ToArrayAsync<TResult>(Expression<Func<TSource, TResult>> selector);
        /// <summary>
        /// 查询单条数据
        /// </summary>
        /// <returns>T 或者 null</returns>
        /// <remarks>查询数据时，自己一定要清楚要查的到底是一条数据还是多条</remarks>
        Task<TResult?> SingleOrDefaultAsync<TResult>(Expression<Func<TSource, TResult>> selector);
        /// <summary>
        /// 查询单条数据
        /// </summary>
        /// <returns>T 或者 null</returns>
        /// <remarks>查询数据时，自己一定要清楚要查的到底是一条数据还是多条</remarks>
        Task<TResult> SingleAsync<TResult>(Expression<Func<TSource, TResult>> selector);
        /// <summary>
        /// 根据id查询单条数据，可能不存在
        /// </summary>
        /// <returns>T 或者 null</returns>
        /// <remarks>查询数据时，自己一定要清楚要查的到底是一条数据还是多条</remarks>
        Task<TResult?> SingleOrDefaultAsync<TResult>(Guid id, Expression<Func<TSource, TResult>> selector);
        /// <summary>
        /// 根据id查询单条数据，一定存在
        /// </summary>
        /// <returns>T 或者 null</returns>
        /// <remarks>查询数据时，自己一定要清楚要查的到底是一条数据还是多条</remarks>
        Task<TResult> SingleAsync<TResult>(Guid id, Expression<Func<TSource, TResult>> selector);
        /// <summary>
        /// 删除一条数据，如果删除数据超过1条，将抛出一场
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// 删除数据时，自己心里一定要明确到底要删几条数据
        /// </remarks>
        Task DeleteAsync();
        /// <summary>
        /// 删除指定ID的数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteAsync(Guid id);
        /// <summary>
        /// 删除符合条件的所有数据
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// 慎用：确信业务需要删除多条数据才能使用此方法
        /// </remarks>
        Task<int> DeleteAllAsync();
        /// <summary>
        /// 批量删除指定ID的数据,如果删除的数据数量与指定的ids的数量不同会报错
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task DeleteAsync(Guid[] ids);
        /// <summary>
        /// 更新一条数据，如果更新数据[不是]1条，将抛出异常
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// 更新数据时，自己心里一定要明确到底要更新几条数据
        /// </remarks>
        Task UpdateAsync(Action<TSource> updateExpression);
        /// <summary>
        /// 更新指定ID的数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updateExpression"></param>
        /// <returns></returns>
        Task UpdateAsync(Guid id, Action<TSource> updateExpression);
        /// <summary>
        /// 更新当前Set已经修改过的数据
        /// </summary>
        Task<int> UpdateAsync();
        /// <summary>
        /// 更新指定ID的数据
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="updateExpression"></param>
        /// <returns></returns>
        Task UpdateAsync(Guid[] ids, Action<TSource> updateExpression);
        /// <summary>
        /// 新增一条数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        Task<Guid> AddAsync(TSource data);
        /// <summary>
        /// 批量新增数据
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        Task AddAsync(TSource[] datas);
        IReadWriteQueryable<TSource> OrderBy<TProperty>(Expression<Func<TSource, TProperty>> predicate);
        IReadWriteQueryable<TSource> OrderByDesc<TProperty>(Expression<Func<TSource, TProperty>> predicate);
    }
}