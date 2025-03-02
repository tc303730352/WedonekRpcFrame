namespace WeDonekRpc.Model
{
    /// <summary>
    /// 广播信息
    /// </summary>
    [IRemoteConfig("Broadcast", "sys.sync", false, true, IsProhibitTrace = true)]
    public class BroadcastMsg
    {
        public BroadcastMsg ()
        {

        }
        public BroadcastMsg (IRemoteBroadcast config)
        {
            this.MsgKey = config.SysDictate;
            this.IsLimitOnly = config.IsOnly;
            this.TypeVal = config.TypeVal;
            this.ServerId = config.ServerId;
            this.IsExclude = config.IsExclude;
            this.IsProhibitTrace = config.IsProhibitTrace;
            this.RegionId = config.RegionId;
            this.IsCrossGroup = config.IsCrossGroup;
            this.RpcMerId = config.RpcMerId;
            if (config.RemoteConfig != null)
            {
                this.MsgConfig = new BasicRemoteConfig(config.RemoteConfig);
            }
        }
        /// <summary>
        /// 是否跨集群组
        /// </summary>
        public bool IsCrossGroup
        {
            get;
            set;
        }
        /// <summary>
        /// 是否排除来源
        /// </summary>
        public bool IsExclude
        {
            get;
            set;
        }
        /// <summary>
        /// 是否禁止链路跟踪
        /// </summary>
        public bool IsProhibitTrace { get; set; }

        /// <summary>
        /// 消息关键字
        /// </summary>
        public string MsgKey
        {
            get;
            set;
        }

        /// <summary>
        /// 消息体
        /// </summary>
        public string MsgBody
        {
            get;
            set;
        }
        /// <summary>
        /// 广播范围
        /// </summary>
        public string[] TypeVal
        {
            get;
            set;
        }
        /// <summary>
        /// 限制接收者来源唯一
        /// </summary>
        public bool IsLimitOnly
        {
            get;
            set;
        }
        /// <summary>
        /// 消息配置
        /// </summary>
        public BasicRemoteConfig MsgConfig
        {
            get;
            set;
        }
        /// <summary>
        /// 限定的服务集群
        /// </summary>
        public long? RpcMerId { get; set; }
        /// <summary>
        /// 范围Id
        /// </summary>
        public int? RegionId { get; set; }
        /// <summary>
        /// 广播的服务器ID
        /// </summary>
        public long[] ServerId { get; set; }
    }
}
