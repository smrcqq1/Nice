using Nice.Entities;
using Nice.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nice.Redis
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// 将数据存储到redis,但是提供和EFCore一样的访问方式
    /// 1.开发人员不关心数据到底在哪里
    /// 2.规范redis的使用
    /// </remarks>
    public class RedisCore : IORM
    {
        public IReadOnlyQueryable<TSource> ReadOnlySet<TSource>(bool canWrite) where TSource : class, IEntitybase
        {
            throw new NotImplementedException();
        }

        public IReadWriteQueryable<TSource> Set<TSource>() where TSource : class, IEntitybase
        {
            throw new NotImplementedException();
        }
    }
}