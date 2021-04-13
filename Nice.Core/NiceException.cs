using System;
using System.Collections.Generic;
using System.Text;

namespace Nice
{
    public class NiceException:Exception
    {
        private NiceException(string message):base(message)
        {

        }
        /// <summary>
        /// 统一异常的抛出机制
        /// </summary>
        /// <param name="message"></param>
        /// <remarks>
        /// 可以在真实和测试环境自由切换
        /// 还可以根据异常级别进行针对处理
        /// </remarks>
        public static void Throw(string message)
        {
#if DEBUG
            throw new NiceException(message);
#else
            Console.WriteLine(message);
#endif
        }
    }
}