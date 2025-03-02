using SqlSugar;

namespace DataBasicCli.RpcService
{
    /// <summary>
    /// 服务节点
    /// </summary>
    [SugarIndex("IX_ContainerId", "ContainerId", OrderByType.Asc, false)]
    [SugarIndex("IX_RemoteServerConfig", "SystemType", OrderByType.Asc, "ServerMac", OrderByType.Asc, "ServerIndex", OrderByType.Asc, true)]
    [SugarTable("RemoteServerConfig", TableDescription = "服务节点配置表")]
    public class RemoteServerConfigModel
    {
        /// <summary>
        /// 服务节点Id
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, ColumnDescription = "节点ID")]
        public long Id { get; set; }

        /// <summary>
        /// 节点内部编号
        /// </summary>
        [SugarColumn(ColumnDescription = "服务内部编号(自定义)", Length = 50, IsNullable = true)]
        public string ServerCode { get; set; }
        /// <summary>
        /// 服务名
        /// </summary>
        [SugarColumn(ColumnDescription = "服务名", Length = 50, IsNullable = false)]
        public string ServerName
        {
            get;
            set;
        }
        /// <summary>
        /// 服务Ip
        /// </summary>
        [SugarColumn(ColumnDescription = "内网访问IP", Length = 36, ColumnDataType = "varchar")]
        public string ServerIp
        {
            get;
            set;
        }
        /// <summary>
        /// 远程Ip
        /// </summary>
        [SugarColumn(ColumnDescription = "跨机房访问IP", Length = 36, ColumnDataType = "varchar")]
        public string RemoteIp
        {
            get;
            set;
        }
        /// <summary>
        /// 服务端口
        /// </summary>
        [SugarColumn(ColumnDescription = "服务端口")]
        public int ServerPort
        {
            get;
            set;
        }
        /// <summary>
        /// 服务端口
        /// </summary>
        [SugarColumn(ColumnDescription = "远程Ip端口号")]
        public int RemotePort
        {
            get;
            set;
        }
        /// <summary>
        /// 服务组别Id
        /// </summary>
        [SugarColumn(ColumnDescription = "服务组别Id")]
        public long GroupId
        {
            get;
            set;
        }
        /// <summary>
        /// 服务类别Id
        /// </summary>
        [SugarColumn(ColumnDescription = "服务类别Id")]
        public long SystemType
        {
            get;
            set;
        }

        /// <summary>
        /// 服务Mac
        /// </summary>
        [SugarColumn(ColumnDescription = "服务MAC地址", Length = 17, ColumnDataType = "varchar")]
        public string ServerMac
        {
            get;
            set;
        }
        /// <summary>
        /// 服务编号
        /// </summary>
        [SugarColumn(ColumnDescription = "服务编号")]
        public int ServerIndex
        {
            get;
            set;
        }
        /// <summary>
        /// 服务类型
        /// </summary>
        [SugarColumn(ColumnDescription = "服务类型")]
        public byte ServiceType
        {
            get;
            set;
        }
        /// <summary>
        /// 拥有的集群ID(登陆)
        /// </summary>
        [SugarColumn(ColumnDescription = "拥有该节点的集群ID")]
        public long HoldRpcMerId
        {
            get;
            set;
        }

        /// <summary>
        /// 容器ID
        /// </summary>
        [SugarColumn(ColumnDescription = "所属容器ID")]
        public long? ContainerId
        {
            get;
            set;
        }
        /// <summary>
        /// 所属容器组ID
        /// </summary>
        [SugarColumn(ColumnDescription = "所属容器组ID", IsNullable = true)]
        public long? ContainerGroupId
        {
            get;
            set;
        }
        /// <summary>
        /// 是否承载在容器中
        /// </summary>
        [SugarColumn(ColumnDescription = "是否由容器承载")]
        public bool IsContainer
        {
            get;
            set;
        }
        /// <summary>
        /// 公有秘钥
        /// </summary>
        [SugarColumn(ColumnDescription = "公有秘钥", Length = 32, ColumnDataType = "varchar")]
        public string PublicKey
        {
            get;
            set;
        }
        /// <summary>
        /// 服务节点状态
        /// </summary>
        [SugarColumn(ColumnDescription = "服务节点状态")]
        public byte ServiceState
        {
            get;
            set;
        }


        /// <summary>
        /// 节点所在区域
        /// </summary>
        [SugarColumn(ColumnDescription = "所在机房")]
        public int RegionId
        {
            get;
            set;
        }
        /// <summary>
        /// 是否在线
        /// </summary>
        [SugarColumn(ColumnDescription = "是否在线")]
        public bool IsOnline
        {
            get;
            set;
        }
        /// <summary>
        /// 链接的服务中心Id
        /// </summary>
        [SugarColumn(ColumnDescription = "上线的服务中心ID")]
        public int BindIndex
        {
            get;
            set;
        }
        /// <summary>
        /// 远程配置优先级
        /// </summary>
        [SugarColumn(ColumnDescription = "下发配置项的优先级")]
        public short ConfigPrower
        {
            get;
            set;
        }
        /// <summary>
        /// 节点熔断恢复后临时限流量
        /// </summary>
        [SugarColumn(ColumnDescription = "熔断恢复后临时流量")]
        public int RecoveryLimit
        {
            get;
            set;
        }
        /// <summary>
        /// 节点熔断恢复后临时限流时长
        /// </summary>
        [SugarColumn(ColumnDescription = "熔断恢复后临时限流时长")]
        public int RecoveryTime
        {
            get;
            set;
        }
        /// <summary>
        /// 服务节点实际连接中控服务的Ip
        /// </summary>
        [SugarColumn(ColumnDescription = "节点链接服务中心的实际IP", Length = 36, ColumnDataType = "varchar")]
        public string ConIp
        {
            get;
            set;
        }
        /// <summary>
        /// 客户端版本号
        /// </summary>
        [SugarColumn(ColumnDescription = "客户端版本号")]
        public int ApiVer
        {
            get;
            set;
        }
        /// <summary>
        /// 版本号
        /// </summary>
        [SugarColumn(ColumnDescription = "应用版本号")]
        public int VerNum
        {
            get;
            set;
        }
        /// <summary>
        /// 最后离线日期
        /// </summary>
        [SugarColumn(ColumnDescription = "最后离线日期", ColumnDataType = "date")]
        public DateTime LastOffliceDate
        {
            get;
            set;
        }
        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(ColumnDescription = "备注", IsNullable = true, Length = 150)]
        public string Remark
        {
            get;
            set;
        }
        /// <summary>
        /// 添加时间
        /// </summary>
        [SugarColumn(ColumnDescription = "添加时间")]
        public DateTime AddTime
        {
            get;
            set;
        }
    }
}
