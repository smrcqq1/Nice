namespace Nice
{
    /// <summary>
    /// 标记一个数据表映射模型
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    public interface IDataTable
    {
        /// <summary>
        /// 凡是数据表必须有ID
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 记录数据的创建时间,默认按照创建时间逆序查询
        /// </summary>
        //todo:列表和分页查询默认按照此字段逆序查询
        public DateTime CreateTime { get; set; }
    }
}