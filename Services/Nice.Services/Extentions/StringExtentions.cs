namespace System
{
    /// <summary>
    /// 字符串扩展
    /// </summary>
    public static class MyStringExtentions
    {
        /// <summary>
        /// 移除字符串末尾的指定字符串（如果有）
        /// </summary>
        /// <param name="src"></param>
        /// <param name="remove"></param>
        /// <returns></returns>
        public static string RemoveEnd(this string src, string remove)
        {
            if (src.EndsWith(remove))
            {
                return src.Remove(src.LastIndexOf(remove));
            }
            return src;
        }
        /// <summary>
        /// 移除字符串开头的指定字符串（如果有）
        /// </summary>
        /// <param name="src"></param>
        /// <param name="remove"></param>
        /// <returns></returns>
        public static string RemoveStart(this string src, string remove)
        {
            if (src.StartsWith(remove))
            {
                return src.Remove(0, remove.Length);
            }
            return src;
        }
    }
}