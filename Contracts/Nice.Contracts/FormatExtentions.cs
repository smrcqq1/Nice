namespace System
{
    public static class FormatExtentions
    {
        public static string ToTime(this DateTime target,string format = "yyyy-MM-dd HH:mm:ss")
        {
            return target.ToString(format);
        }
        public static string ToTime(this DateTime? target, string format = "yyyy-MM-dd HH:mm:ss")
        {
            if(target == null)
            {
                return string.Empty;
            }
            return target.Value.ToTime(format);
        }
        public static string ToDate(this DateTime target, string format = "yyyy-MM-dd")
        {
            return target.ToString(format);
        }
        public static string ToDate(this DateTime? target, string format = "yyyy-MM-dd")
        {
            if (target == null)
            {
                return string.Empty;
            }
            return target.Value.ToDate(format);
        }
    }
}
