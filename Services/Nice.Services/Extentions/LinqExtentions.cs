namespace Nice
{
    /// <summary>
    /// 对linq进行的一些扩展
    /// </summary>
    public static class LinqExtentions
    {
        /// <summary>
        /// 对linq进行扩展
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static System.Reflection.MethodInfo GetMethodInfo<TInterface>(this Expression<Func<TInterface, MulticastDelegate>> selector)
        {
            var tt = selector.Body as UnaryExpression;
            var ttt = tt!.Operand as MethodCallExpression;
            var tttt = ttt!.Object as ConstantExpression;
            var v = tttt!.Value as System.Reflection.MethodInfo;
            return v!;
        }
    }
}