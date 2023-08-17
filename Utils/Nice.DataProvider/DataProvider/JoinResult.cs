namespace Nice.DataProvider
{
    /// <summary>
    /// Join结果
    /// </summary>
    /// <typeparam name="TInner"></typeparam>
    /// <typeparam name="TOuter"></typeparam>
    public class JoinResult<TInner,TOuter>
    {
        /// <summary>
        /// 
        /// </summary>
#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
        public TInner Inner { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public TOuter Outer { get; set; }
    }
}
