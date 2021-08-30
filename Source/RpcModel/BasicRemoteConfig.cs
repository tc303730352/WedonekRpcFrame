namespace RpcModel
{
        /// <summary>
        /// 广播消息配置
        /// </summary>
        public class BasicRemoteConfig
        {
                public BasicRemoteConfig()
                {

                }
                public BasicRemoteConfig(IRemoteConfig config)
                {
                        this.TransmitType = config.TransmitType;
                        this.LockColumn = config.LockColumn;
                        this.IsSync = config.IsSync;
                        this.LockType = config.LockType;
                        this.IsAllowRetry = config.IsAllowRetry;
                        this.IdentityColumn = config.IdentityColumn;
                        this.ZIndexBit = config.ZIndexBit;
                }
                /// <summary>
                /// 负载均衡的计算方式
                /// </summary>
                public TransmitType TransmitType
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
                /// 事务Id
                /// </summary>
                public int TransmitId { get; set; }
        }
}
