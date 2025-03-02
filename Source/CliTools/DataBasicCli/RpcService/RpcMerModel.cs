using SqlSugar;

namespace DataBasicCli.RpcService
{
    [SugarIndex("IX_AppId", "AppId", OrderByType.Asc, true)]
    [SugarTable("RpcMer", TableDescription = "服务集群配置表")]
    public class RpcMerModel
    {
        /// <summary>
        /// 集群Id
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public long Id
        {
            get;
            set;
        }
        /// <summary>
        /// 系统名
        /// </summary>
        [SugarColumn(ColumnDescription = "集群名", Length = 50, IsNullable = false)]
        public string SystemName
        {
            get;
            set;
        }
        /// <summary>
        /// 应用APPId
        /// </summary>
        [SugarColumn(ColumnDescription = "集群标识AppId", Length = 32, ColumnDataType = "varchar")]
        public string AppId
        {
            get;
            set;
        }
        /// <summary>
        /// 应用秘钥
        /// </summary>
        [SugarColumn(ColumnDescription = "集群登陆密钥", Length = 32, ColumnDataType = "varchar")]
        public string AppSecret
        {
            get;
            set;
        }
        /// <summary>
        /// 允许访问的服务器IP
        /// </summary>
        [SugarColumn(ColumnDescription = "允许链接的IP地址列表")]
        public string AllowServerIp
        {
            get;
            set;
        }
        [SugarColumn(ColumnDescription = "添加时间")]
        public DateTime AddTime
        {
            get;
            set;
        }
    }
}
