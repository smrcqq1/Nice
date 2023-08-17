namespace Nice.Simple.DataModel
{
    /// <summary>
    /// 学生表
    /// </summary>
    /// <remarks>
    /// 必须打上表格标记
    /// 这一层可以由架构师统一掌握，也可以酌情让开发自行掌握
    /// </remarks>
    public class Student:IDataTable
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 身份证号码
        /// </summary>
        public string IDCard { get; set; }
        /// <summary>
        /// 照片
        /// </summary>
        public string Image { get; set; }
        /// <summary>
        /// 电话号码
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 该学生的班主任ID
        /// </summary>
        public virtual Guid? TeacherID { get; set; }
        /// <summary>
        /// 该学生的班主任
        /// </summary>
        public virtual Teacher? Teacher { get; set; }
        public Guid Id { get; set; }
        public DateTime CreateTime { get; set; }

        public virtual List<获奖记录> Records { get; set; }
    }
}