#region using
using Nice;
using Nice.Entities;
using Nice.ORM;
using System.Collections.Generic;
#endregion using
namespace Test.Database.Entities
{
    /// <summary>
    /// 数据模型的扩展性样例,演示框架的所有功能,所以没有基础封装
    /// </summary>
    public class Student : IEntitybase
        ,ITenated
        //, INamedItem
        , IOperationRecord
        ,ISoftDeletable
        //, ITreeable<Student>
    {
        #region (IEntitybase)主键
        public int ID { get; set; }
        #endregion (IEntitybase)主键

        #region (ITenated)租户ID,这个通常在一个项目中使用的时候是统一的,所以提供了TenatedEntity的封装
        public int TenatID { get; set; }
        #endregion (ITenated)租户ID,这个通常在一个项目中使用的时候是统一的,所以提供了TenatedEntity的封装

        #region (INamedItem)快捷转换为下拉框的数据源使用
        public string Name { get; set; }
        #endregion (INamedItem)快捷转换为下拉框的数据源使用

        #region (IOperationRecord)简单的操作记录,只记录增删改的时间,详细记录请使用IModifyRecord
        public virtual List<OperationRecord> OperationRecords { get; set; }
        #endregion (IOperationRecord)简单的操作记录,只记录增删改的时间

        #region (ISoftDeletable)逻辑删除标记
        public bool IsDeleted { get; set; }
        #endregion (ISoftDeletable)逻辑删除标记

        #region [ITreeable]无限层级树形结构
        //public virtual List<Student> Children { get; set; }
        //public int ParentID { get; set; }
        //public virtual Student Parent { get; set; }
        #endregion [ITreeable]无限层级树形结构

        #region 关系
        //模型的关系通常较多,为了避免杂乱,请尽量像下面这样将与同一模型的关系放到一个region下,这样看的时候不会被其它关系干扰
        #region 与教师关系
        public virtual int TeacherID { get; set; }
        public virtual Teacher Teacher { get; set; }
        #endregion 与教师关系

        #region 与班级关系
        #endregion 与班级关系

        #region 与校内组织关系
        #endregion 与校内组织关系
        #endregion 关系
    }
}