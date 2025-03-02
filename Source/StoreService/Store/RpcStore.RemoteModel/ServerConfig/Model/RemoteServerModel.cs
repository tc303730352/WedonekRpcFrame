using WeDonekRpc.Model;

namespace RpcStore.RemoteModel.ServerConfig.Model
{
    public class RemoteServerModel
    {
        /// <summary>
        /// 服务ID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 服务编号
        /// </summary>
        public string ServerCode { get; set; }
        /// <summary>
        /// 服务名
        /// </summary>
        public string ServerName
        {
            get;
            set;
        }
        /// <summary>
        /// 服务Ip
        /// </summary>
        public string ServerIp
        {
            get;
            set;
        }
        /// <summary>
        /// 远程Ip
        /// </summary>
        public string RemoteIp
        {
            get;
            set;
        }
        /// <summary>
        /// 服务端口
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
        /// 服务组别Id
        /// </summary>
        public long GroupId
        {
            get;
            set;
        }
        /// <summary>
        /// 服务类别Id
        /// </summary>
        public long SystemType
        {
            get;
            set;
        }

        /// <summary>
        /// 服务Mac
        /// </summary>
        public string ServerMac
        {
            get;
            set;
        }
        /// <summary>
        /// 服务编号
        /// </summary>
        public int ServerIndex
        {
            get;
            set;
        }
        /// <summary>
        /// 服务类型
        /// </summary>
        public RpcServerType ServiceType
        {
            get;
            set;
        }
        /// <summary>
        /// 拥有的集群ID(登陆)
        /// </summary>
        public long HoldRpcMerId
        {
            get;
            set;
        }
        /// <summary>
        /// 拥有的集群名
        /// </summary>
        public string HoldRpcMer { get; set; }
        /// <summary>
        /// 容器ID
        /// </summary>
        public long? ContainerId
        {
            get;
            set;
        }
        /// <summary>
        /// 是否承载在容器中
        /// </summary>
        public bool IsContainer
        {
            get;
            set;
        }
        /// <summary>
        /// 公有秘钥
        /// </summary>
        public string PublicKey
        {
            get;
            set;
        }
        /// <summary>
        /// 服务节点状态
        /// </summary>
        public RpcServiceState ServiceState
        {
            get;
            set;
        }

        /// <summary>
        /// 权值
        /// </summary>
        public int Weight
        {
            get;
            set;
        }

        /// <summary>
        /// 节点所在区域
        /// </summary>
        public int RegionId
        {
            get;
            set;
        }
        /// <summary>
        /// 是否在线
        /// </summary>
        public bool IsOnline
        {
            get;
            set;
        }
        /// <summary>
        /// 链接的服务中心Id
        /// </summary>
        public int BindIndex
        {
            get;
            set;
        }
        /// <summary>
        /// 远程配置优先级
        /// </summary>
        public short ConfigPrower
        {
            get;
            set;
        }
        /// <summary>
        /// 节点熔断恢复后临时限流量
        /// </summary>
        public int RecoveryLimit
        {
            get;
            set;
        }
        /// <summary>
        /// 节点熔断恢复后临时限流时长
        /// </summary>
        public int RecoveryTime
        {
            get;
            set;
        }
        /// <summary>
        /// 服务节点实际连接中控服务的Ip
        /// </summary>
        public string ConIp
        {
            get;
            set;
        }
        /// <summary>
        /// 客户端版本号
        /// </summary>
        public int ApiVer
        {
            get;
            set;
        }
        /// <summary>
        /// 版本号
        /// </summary>
        public int VerNum
        {
            get;
            set;
        }
        /// <summary>
        /// 最后离线日期
        /// </summary>
        public DateTime LastOffliceDate
        {
            get;
            set;
        }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddTime
        {
            get;
            set;
        }
    }
}
