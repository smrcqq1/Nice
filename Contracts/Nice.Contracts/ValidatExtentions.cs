using System.Linq;

namespace Nice
{
    public static class ValidatExtentions
    {
        public static int ParameterErrorCode = 400;
        static void ThrowParameterError(params string[] message)
        {
            throw new Nice.Exception(ParameterErrorCode, message);
        }
        static void ThrowParameterError(List<string> message)
        {
            throw new Nice.Exception(ParameterErrorCode, message);
        }
        /// <summary>
        /// 整数检查
        /// </summary>
        /// <param name="target"></param>
        /// <param name="displayname"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int Between(this int target, string displayname, int min, int max)
        {
            if (target < min)
            {
                ThrowParameterError($"{displayname}不能小于{min}");
            }
            if (target > max)
            {
                ThrowParameterError($"{displayname}不能大于{max}");
            }
            return target;
        }
        /// <summary>
        /// 可空整数检查
        /// </summary>
        /// <param name="target"></param>
        /// <param name="displayname"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int? Between(this int? target, string displayname, int min, int max)
        {
            if (target == null)
            {
                ThrowParameterError($"{displayname}不能为空");
            }
            return target!.Value.Between(displayname, min, max);
        }
        /// <summary>
        /// 时间检查
        /// </summary>
        /// <param name="target"></param>
        /// <param name="displayname"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static DateTime? Between(this DateTime? target, string displayname, DateTime min, DateTime max)
        {
            if (target == null)
            {
                ThrowParameterError($"{displayname}不能为空");
            }
            return target!.Value.Between(displayname, min, max);
        }
        public static DateTime? Between(this DateTime? target, string displayname)
        {
            return target.Between(displayname, DateTime.MinValue);
        }
        public static DateTime? Between(this DateTime? target, string displayname, DateTime min)
        {
            return target.Between(displayname, min, DateTime.MaxValue);
        }
        /// <summary>
        /// 时间检查
        /// </summary>
        /// <param name="target"></param>
        /// <param name="displayname"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static DateTime Between(this DateTime target, string displayname, DateTime min, DateTime max)
        {
            if (target == null || target == new DateTime())
            {
                ThrowParameterError($"{displayname}不能为空");
            }
            if (target < min)
            {
                ThrowParameterError($"{displayname}不能早于{min.ToTime()}");
            }
            if (target > max)
            {
                ThrowParameterError($"{displayname}不能晚于{max.ToTime()}");
            }
            return target;
        }
        public static DateTime Between(this DateTime target, string displayname, DateTime min)
        {
            return target.Between(displayname,min,DateTime.MaxValue);
        }
        public static DateTime Between(this DateTime target, string displayname)
        {
            return target.Between(displayname, DateTime.MinValue);
        }
        /// <summary>
        /// 字符串检查
        /// </summary>
        /// <param name="target">目标字符串</param>
        /// <param name="displayname">错误提示文本</param>
        /// <param name="min">最小值,默认2</param>
        /// <param name="max">最大值,默认100</param>
        public static string Between(this string target, string displayname, int min = 2, int max = 100)
        {
            if (string.IsNullOrEmpty(target))
            {
                ThrowParameterError($"{displayname}不能为空");
            }
            if (target.Length < min)
            {
                ThrowParameterError($"{displayname}至少需要{min}个字符");
            }
            if (target.Length > max)
            {
                ThrowParameterError($"{displayname}最多{max}个字符");
            }
            return target;
        }
        /// <summary>
        /// Guid非空检查
        /// </summary>
        /// <param name="target"></param>
        /// <param name="displayname"></param>
        /// <returns></returns>
        public static Guid NotNull(this Guid target, string displayname = "id")
        {
            if (target == null || target == Guid.Empty)
            {
                ThrowParameterError($"{displayname}不能为空");
            }
            return target;
        }
        /// <summary>
        /// Guid非空检查
        /// </summary>
        /// <param name="target"></param>
        /// <param name="displayname"></param>
        /// <returns></returns>
        public static Guid? NotNull(this Guid? target, string displayname = "id")
        {
            if (target == null || target == Guid.Empty)
            {
                ThrowParameterError($"{displayname}不能为空");
            }
            return target;
        }
        /// <summary>
        /// 数组检查
        /// </summary>
        /// <param name="target"></param>
        /// <param name="displayname"></param>
        /// <returns></returns>
        public static IEnumerable<T> NotNull<T>(this IEnumerable<T> target, string displayname)
        {
            if (target == null || !target.Any())
            {
                ThrowParameterError($"{displayname}不能为空");
            }
            return target!;
        }
        /// <summary>
        /// 对数组每一项进行检查,会自动检查空数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="targets"></param>
        /// <param name="displayname"></param>
        /// <param name="checkOne"></param>
        /// <returns></returns>
        public static IEnumerable<T> Foreach<T>(this IEnumerable<T> targets, string displayname, Func<T, string> checkOne)
        {
            targets.NotNull(displayname);
            var errs = new List<string>();
            foreach (var item in targets)
            {
                var str = checkOne(item);
                if (!string.IsNullOrEmpty(str))
                {
                    errs.Add(str);
                }
            }
            if (errs.Any())
            {
                ThrowParameterError(errs);
            }
            return targets;
        }
    }
}