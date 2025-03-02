using WeDonekRpc.Model;

namespace RpcStore.Model.ServerGroup
{
    public class BindServerGroup
    {
        /// <summary>
        /// 数据Id
        /// </summary>
        public long BindId
        {
            get;
            set;
        }
        /// <summary>
        /// 服务Id
        /// </summary>
        public long ServerId
        {
            get;
            set;
        }
        /// <summary>
        /// 区域ID
        /// </summary>
        public int RegionId
        {
            get;
            set;
        }

        /// <summary>
        /// 节点类型
        /// </summary>
        public long SystemType
        {
            get;
            set;
        }

        /// <summary>
        /// 权重
        /// </summary>
        public int Weight
        {
            get;
            set;
        }
        /// <summary>
        /// 服务名
        /// </summary>
        public string ServerName { get; set; }
        /// <summary>
        /// 服务组
        /// </summary>
        public long GroupId { get; set; }
        /// <summary>
        /// 是否为容器
        /// </summary>
        public bool IsContainer { get; set; }

        /// <summary>
        /// 服务节点状态
        /// </summary>
        public RpcServiceState ServiceState
        {
            get;
            set;
        }
        public bool IsOnline { get; set; }

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
        /// 容器ID
        /// </summary>
        public long? ContainerId
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
        /// 服务Ip
        /// </summary>
        public string ServerIp
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
        public bool IsHold
        {
            get;
            set;
        }
    }
}
