namespace WeDonekRpc.Model
{
    public class RpcServerConfig
    {
        /// <summary>
        /// 服务名
        /// </summary>
        public string Name
        {
            get;
            set;
        }
        /// <summary>
        /// 服务器ID
        /// </summary>
        public long ServerId
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
        /// 服务端端口
        /// </summary>
        public int ServerPort
        {
            get;
            set;
        }
        /// <summary>
        /// 公钥Key
        /// </summary>
        public string PublicKey
        {
            get;
            set;
        }
        /// <summary>
        /// 所属服务组
        /// </summary>
        public string GroupTypeVal
        {
            get;
            set;
        }
        /// <summary>
        /// 服务组Id
        /// </summary>
        public long GroupId
        {
            get;
            set;
        }
        /// <summary>
        /// 节点类型ID
        /// </summary>
        public long SystemType
        {
            get;
            set;
        }
        /// <summary>
        /// 服务状态
        /// </summary>
        public RpcServiceState ServiceState
        {
            get;
            set;
        }
        /// <summary>
        /// 所属容器组
        /// </summary>
        public long? ContainerGroup { get; set; }
        /// <summary>
        /// 所属区域
        /// </summary>
        public int RegionId { get; set; }
        /// <summary>
        /// 熔断恢复限制数
        /// </summary>
        public int RecoveryLimit { get; set; }

        /// <summary>
        /// 熔断恢复限制时间(秒)
        /// </summary>
        public int RecoveryTime { get; set; }
        /// <summary>
        /// 配置权限值
        /// </summary>
        public short ConfigPrower
        {
            get;
            set;
        }
        /// <summary>
        /// 节点版本号
        /// </summary>
        public int VerNum { get; set; }
    }
}
