using System;
using System.Collections.Generic;
using System.Text;

namespace Nice.Cache
{
    /// <summary>
    /// 静态化封装.缓存有时候会使用多种类型,所以封装需要单独处理
    /// </summary>
    public interface IStaticize:ICache
    {
    }
}
