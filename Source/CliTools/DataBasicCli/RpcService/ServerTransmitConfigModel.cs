using SqlSugar;

namespace DataBasicCli.RpcService
{
    [SugarTable("ServerTransmitConfig", TableDescription = "负载均衡配置")]
    [SugarIndex("IX_SchemeId", "SchemeId", OrderByType.Asc, false)]
    [SugarIndex("IX_SchemeId_ServerCode", "SchemeId", OrderByType.Asc, "ServerCode", OrderByType.Asc, true)]
    public class ServerTransmitConfigModel
    {
        [SugarColumn(IsPrimaryKey = true, ColumnDescription = "标识ID")]
        public long Id
        {
            get;
            set;
        }
        [SugarColumn(ColumnDescription = "方案ID")]
        public long SchemeId
        {
            get;
            set;
        }
        /// <summary>
        /// 服务节点Id
        /// </summary>
        [SugarColumn(ColumnDescription = "服务节点编号", ColumnDataType = "varchar", Length = 50)]
        public string ServerCode
        {
            get;
            set;
        }

        /// <summary>
        /// 负载配置
        /// </summary>
        [SugarColumn(ColumnDescription = "负载配置", ColumnDataType = "varchar")]
        public string TransmitConfig
        {
            get;
            set;
        }
    }
}
