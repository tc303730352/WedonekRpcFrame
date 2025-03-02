using SqlSugar;

namespace DataBasicCli.RpcExtendService
{
    [SugarTable("DictateNode", "广播节点路由配置表")]
    public class DictateNodeModel
    {
        /// <summary>
        /// 数据Id
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, ColumnDescription = "标识ID")]
        public long Id { get; set; }
        /// <summary>
        /// 父级Id
        /// </summary>
        [SugarColumn(ColumnDescription = "父级Id")]
        public long ParentId { get; set; }
        /// <summary>
        /// 指令集
        /// </summary>
        [SugarColumn(ColumnDescription = "消息指令", ColumnDataType = "varchar", Length = 50)]
        public string Dictate { get; set; }
        /// <summary>
        /// 指令名
        /// </summary>
        [SugarColumn(ColumnDescription = "指令名", Length = 50)]
        public string DictateName { get; set; }
        /// <summary>
        /// 是否终节点
        /// </summary>
        [SugarColumn(ColumnDescription = "是否是终节点")]
        public bool IsEndpoint { get; set; }
    }
}
