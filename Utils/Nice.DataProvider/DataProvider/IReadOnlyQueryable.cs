namespace Nice.DataProvider
{
    /// <summary>
    /// 通用的数据提供器
    /// </summary>
    /// <remarks>
    /// 对业务层屏蔽掉数据的持久化，缓存等
    /// </remarks>
    public interface IReadOnlyQueryable<TSource>
    {
        /// <summary>
        /// 如果指定条件为真,则增加一个查询条件
        /// </summary>
        /// <returns></returns>
        IReadOnlyQueryable<TSource> WhereIf(bool condition, Expression<Func<TSource, bool>> expression);
        /// <summary>
        /// 如果指定字符串不为空,则增加一个查询条件
        /// </summary>
        /// <returns></returns>
        IReadOnlyQueryable<TSource> WhereIf(string condition, Expression<Func<TSource, bool>> expression);
        /// <summary>
        /// 如果指定Guid不为空,则增加一个查询条件
        /// </summary>
        /// <returns></returns>
        IReadOnlyQueryable<TSource> WhereIf(Guid? condition, Expression<Func<TSource, bool>> expression);
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IReadOnlyQueryable<TSource> OrderBy<TProperty>(Expression<Func<TSource, TProperty>> predicate);
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IReadOnlyQueryable<TSource> OrderByDesc<TProperty>(Expression<Func<TSource, TProperty>> predicate);
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
        IReadOnlyQueryable<TSource> Where(Expression<Func<TSource, bool>> expression);
        ///// <summary>
        ///// 设置源数据到结果集的映射关系
        ///// </summary>
        ///// <typeparam name="TResult"></typeparam>
        ///// <param name="expression"></param>
        ///// <returns></returns>
        //IReadOnlyQueryable<TResult> Select<TResult>(Expression<Func<T, TResult>> expression);
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
        /// 查询单条数据
        /// </summary>
        /// <returns>T 或者 null</returns>
        /// <remarks>查询数据时，自己一定要清楚要查的到底是一条数据还是多条</remarks>
        Task<TResult?> SingleOrDefaultAsync<TResult>(Expression<Func<TSource,TResult>> selector);
        /// <summary>
        /// 查询单条数据
        /// </summary>
        /// <returns>T 或者 null</returns>
        /// <remarks>查询数据时，自己一定要清楚要查的到底是一条数据还是多条</remarks>
        Task<TResult?> SingleOrDefaultAsync<TResult>(Guid id,Expression<Func<TSource, TResult>> selector);
        /// <summary>
        /// 查询单条数据
        /// </summary>
        /// <returns>T 或者 null</returns>
        /// <remarks>查询数据时，自己一定要清楚要查的到底是一条数据还是多条</remarks>
        Task<TResult> SingleAsync<TResult>(Guid id, Expression<Func<TSource, TResult>> selector);
        /// <summary>
        /// 查询单条数据
        /// </summary>
        /// <returns>T 或者 null</returns>
        /// <remarks>查询数据时，自己一定要清楚要查的到底是一条数据还是多条</remarks>
        Task<TResult> SingleAsync<TResult>(Expression<Func<TSource, TResult>> selector);
        /// <summary>
        /// 查询多条数据
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// 慎用：不允许使用这个方法来查询单条数据，业务一定要理解透
        /// </remarks>
        Task<TResult[]> ToArrayAsync<TResult>(Expression<Func<TSource, TResult>> selector);
    }
}