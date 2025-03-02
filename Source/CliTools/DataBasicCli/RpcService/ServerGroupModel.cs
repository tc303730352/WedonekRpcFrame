using SqlSugar;

namespace DataBasicCli.RpcService
{
    /// <summary>
    /// 服务组
    /// </summary>
    [SugarTable("ServerGroup", TableDescription = "服务组别表")]
    public class ServerGroupModel
    {
        /// <summary>
        /// 服务组ID
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, ColumnDescription = "服务组别ID")]
        public long Id
        {
            get;
            set;
        }
        /// <summary>
        /// 组类别
        /// </summary>
        [SugarColumn(ColumnDescription = "组别值", Length = 50, ColumnDataType = "varchar")]
        public string TypeVal
        {
            get;
            set;
        }
        /// <summary>
        /// 类别名
        /// </summary>
        [SugarColumn(ColumnDescription = "组别名", Length = 50)]
        public string GroupName
        {
            get;
            set;
        }
    }
}
