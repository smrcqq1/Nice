//#region using
//using Microsoft.EntityFrameworkCore;
//using Nice.Entities;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//#endregion using
//namespace Nice.ORM.EFCore
//{
//    /// <summary>
//    /// 对逻辑删除的数据查询封装
//    /// </summary>
//    /// <typeparam name="TSource"></typeparam>
//    public class SoftDeleteQueryable<TSource> : ReadWriteQueryable<TSource> where TSource : class, IEntitybase, ISoftDeletable
//    {
//        #region 构造函数
//        public SoftDeleteQueryable(DbContext dbcontext):base(dbcontext)
//        {
//            source = source.Where(o=>o.IsDeleted == false);
//        }
//        #endregion 构造函数

//        #region 重写删除
//        public override async Task<bool> DeleteAsync()
//        {
//            await source
//                .UpdateFromQueryAsync(o => new { IsDeleted = true });
//            return true;
//        }
//        public override async Task<bool> DeleteAsync(IEnumerable<TSource> datas)
//        {
//            var ids = datas.Select(o => o.ID).ToArray();
//            var cnt = await source
//                .Where(o=> ids.Contains(o.ID))
//                .UpdateFromQueryAsync(o => new { IsDeleted = true });
//            if(cnt != datas.Count())
//            {
//                NiceException.Throw($"批量逻辑删除数据的条目数量不正确,预期删除:{datas.Count()},实际删除:{cnt}");
//            }
//            return true;
//        }
//        #endregion 重写删除
//    }
//    /// <summary>
//    /// 多租户的逻辑删除数据查询封装
//    /// </summary>
//    /// <typeparam name="TSource"></typeparam>
//    public class SoftDeleteTenatedQueryable<TSource> : SoftDeleteQueryable<TSource> where TSource : class, IEntitybase,ITenated, ISoftDeletable
//    {
//        #region 构造函数
//        public SoftDeleteTenatedQueryable(DbContext dbcontext, ITenatedUserinfo userinfo) : base(dbcontext)
//        {
//            source = source.Where(o => o.TenatID == userinfo.TenatID);
//        }
//        #endregion 构造函数
//    }
//    /// <summary>
//    /// 多租户查询封装
//    /// </summary>
//    /// <typeparam name="TSource"></typeparam>
//    public class TenatedQueryable<TSource> : ReadWriteQueryable<TSource> where TSource : class, IEntitybase, ITenated
//    {
//        #region 构造函数
//        public TenatedQueryable(DbContext dbcontext, ITenatedUserinfo userinfo) : base(dbcontext)
//        {
//            source = source.Where(o => o.TenatID == userinfo.TenatID);
//        }
//        #endregion 构造函数
//    }
//}