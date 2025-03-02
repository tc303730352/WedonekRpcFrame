namespace WeDonekRpc.Model
{
    /// <summary>
    /// 广播消息配置
    /// </summary>
    public class BasicRemoteConfig
    {
        public BasicRemoteConfig ()
        {

        }
        public BasicRemoteConfig (RemoteSet config)
        {
            this.TransmitType = config.TransmitType;
            this.LockColumn = config.LockColumn;
            this.IsSync = config.IsEnableLock;
            this.LockType = config.LockType;
            this.IsAllowRetry = config.IsAllowRetry;
            this.IdentityColumn = config.IdentityColumn;
            this.IsProhibitTrace=config.IsProhibitTrace;
            this.ZIndexBit = config.ZIndexBit;
            this.RetryNum = config.RetryNum;
            this.TimeOut = config.TimeOut;
            this.Transmit = config.Transmit;
            this.AppIdentity = config.AppIdentity;
        }
        /// <summary>
        /// 负载均衡的计算方式
        /// </summary>
        public TransmitType TransmitType
        {
            get;
            set;
        }
        public bool IsProhibitTrace
        {
            get;
            set;
        }
        /// <summary>
        /// 标识列(计算负载均衡用,计算zoneIndex,hashcode的列名）
        /// </summary>
        public string IdentityColumn
        {
            get;
            set;
        }
        /// <summary>
        /// 计算Index的索引位
        /// </summary>
        public int[] ZIndexBit { get; set; }

        /// <summary>
        /// 是否同步启动同步锁(解决客户端重复提交问题)
        /// </summary>
        public bool IsSync
        {
            get;
            set;
        }
        /// <summary>
        /// 锁定用提供标识的列名(用于生成锁的唯一标识)
        /// </summary>
        public string[] LockColumn
        {
            get;
            set;
        }
        /// <summary>
        /// 是否即刻重置锁状态
        /// </summary>
        public RemoteLockType LockType
        {
            get;
            set;
        }

        /// <summary>
        /// 是否允许重试
        /// </summary>
        public bool IsAllowRetry
        {
            get;
            set;
        }
        /// <summary>
        /// 重试数
        /// </summary>
        public int? RetryNum { get; set; }
        /// <summary>
        /// 发送超时时间
        /// </summary>
        public int? TimeOut { get; set; }
        /// <summary>
        /// 负载均衡方案
        /// </summary>
        public string Transmit { get; set; }
        /// <summary>
        /// 应用标识列
        /// </summary>
        public string AppIdentity { get; set; }
    }
}
