using WeDonekRpc.Model;

namespace RpcCentral.Model
{
    public class RemoteServerModel
    {
        /// <summary>
        /// 服务名
        /// </summary>
        public string ServerName
        {
            get;
            set;
        }
        /// <summary>
        /// 服务端Ip
        /// </summary>
        public string ServerIp
        {
            get;
            set;
        }
        /// <summary>
        /// 远程IP
        /// </summary>
        public string RemoteIp
        {
            get;
            set;
        }
        /// <summary>
        /// 服务端端口
        /// </summary>
        public int ServerPort
        {
            get;
            set;
        }

        /// <summary>
        /// 远程端口
        /// </summary>
        public int RemotePort
        {
            get;
            set;
        }
        /// <summary>
        /// 链接的公有Key
        /// </summary>
        public string PublicKey
        {
            get;
            set;
        }
        /// <summary>
        /// 所属组ID
        /// </summary>
        public long GroupId { get; set; }

        /// <summary>
        /// 所属系统类型
        /// </summary>
        public long SystemType { get; set; }

        /// <summary>
        /// 拥有的集群ID(登陆)
        /// </summary>
        public long HoldRpcMerId
        {
            get;
            set;
        }
        public long ContainerId
        {
            get;
            set;
        }

        /// <summary>
        /// 容器组id
        /// </summary>
        public long ContainerGroupId { get; set; }
        public bool IsContainer
        {
            get;
            set;
        }
        /// <summary>
        /// API版本号
        /// </summary>
        public int ApiVer { get; set; }

        /// <summary>
        /// 服务状态
        /// </summary>
        public RpcServiceState ServiceState { get; set; }

        /// <summary>
        /// 绑定服务中心编号
        /// </summary>
        public int BindIndex { get; set; }
        /// <summary>
        /// 配置权值(大于10则获取远程配置优先)
        /// </summary>
        public short ConfigPrower { get; set; }
        /// <summary>
        /// 熔断后恢复限流量
        /// </summary>
        public int RecoveryLimit { get; set; }
        /// <summary>
        /// 熔断后恢复限定时间
        /// </summary>
        public int RecoveryTime { get; set; }
        /// <summary>
        /// 当前连接ip
        /// </summary>
        public string ConIp { get; set; }
        /// <summary>
        /// 所属区域
        /// </summary>
        public int RegionId { get; set; }
        /// <summary>
        /// 是否 在线
        /// </summary>
        public bool IsOnline { get; set; }

        /// <summary>
        /// MAC地址
        /// </summary>
        public string ServerMac { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public int VerNum { get; set; }
    }
}
