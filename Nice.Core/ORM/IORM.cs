using Nice.Entities;
using Nice.ORM;

namespace Nice
{
    /// <summary>
    /// ORM框架统一封装
    /// </summary>
    /// <remarks>
    /// 该封装的目的如下:
    /// 1.规范数据库操作的写法,使业务层专注于业务实现
    /// 2.屏蔽不常用的方法,避免业务层由于选择过多导致的混乱
    /// 3.对某些易出错的写法(如不写Select,滥用FirstOrDefault)进行封装和限制,帮助业务开发人员发现逻辑错误,或从技术上(一定程度)避免他们写出错误的逻辑代码
    /// 4.提供统一的扩展方法,如批量操作,读写分离,逻辑删除,多租户等,使业务层不关心这些问题,从而简化业务逻辑,提高开发效率
    /// 5.面向接口编程,可以平滑升级,不担心框架升级带来的影响
    /// </remarks>
    public interface IORM
    {
        /// <summary>
        /// 获取一个数据集的[可读可写]辅助类
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <returns></returns>
        IReadWriteQueryable<TSource> Set<TSource>() where TSource : class, IEntitybase;

        /// <summary>
        /// 获取一个数据集的[只读]辅助类
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="canWrite">是否只读</param>
        /// <returns></returns>
        /// <remarks>
        /// 封装目的:
        /// 1.从代码层面做读写分离=>成本最低
        /// </remarks>
        IReadOnlyQueryable<TSource> ReadOnlySet<TSource>(bool canWrite = false) where TSource : class, IEntitybase;
    }
}