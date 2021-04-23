using MongoDB.Bson;
using Nice;
using Nice.Entities;
using Nice.ORM;
using System.Collections.Generic;

namespace Test.Database.Entities
{
    /// <summary>
    /// 正常在项目中使用数据模型的样例
    /// </summary>
    /// <remarks>
    /// 真正业务系统中,要通过定义一些基类来管理统一的东西如多租户,这样只需要修改基类即可实现多租户与单租户的切换等操作
    /// 还有如逻辑删除,操作记录等都可以封装到统一的基础模型,但是为了减少性能损失,这些接口推荐不要统一封装,而是根据实际需要对每个数据模型进行针对性设计
    /// </remarks>
    public class Teacher : TenatedEntity, INamedItem, IModifyRecord, ISoftDeletable
    {
        public ObjectId _id { get; set; }
        #region (IModifyRecord)详细的操作记录,除了记录增删改的时间,还会记录修改操作的字段,旧值和新值
        public virtual List<DetailOperationRecord> OperationRecords { get; set; }
        #endregion (IModifyRecord)详细的操作记录,除了记录增删改的时间,还会记录修改操作的字段,旧值和新值

        #region (ISoftDeletable)逻辑删除标记
        public bool IsDeleted { get; set; }
        #endregion (ISoftDeletable)逻辑删除标记

        #region (INamedItem)快捷转换为下拉框的数据源使用
        public string Name { get; set; }
        #endregion (INamedItem)快捷转换为下拉框的数据源使用

        #region 关系
        #region 与Student关系
        public virtual List<Student> Students { get; set; }
        #endregion 与Student关系
        #endregion 关系
    }
}
