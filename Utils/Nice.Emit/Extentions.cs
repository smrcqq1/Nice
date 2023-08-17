#region using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion using
namespace Nice.Emit
{
    /// <summary>
    /// Emit相关扩展
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    public static class Extentions
    {
        /// <summary>
        /// 获取一个代理工厂
        /// </summary>
        /// <returns></returns>
        public static IProxyFactory GetBuilder(string name)
        {
            return new ProxyFactory(name);
        }
    }
}