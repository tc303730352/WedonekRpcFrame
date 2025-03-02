using WeDonekRpc.Model;

namespace WeDonekRpc.ExtendModel.RetryTask.Model
{
    public class RpcRemoteSet
    {
        /// <summary>
        /// 负载均衡的计算方式
        /// </summary>
        public TransmitType TransmitType
        {
            get;
            set;
        }
        /// <summary>
        /// 是否禁止链路
        /// </summary>
        public bool IsProhibitTrace { get; set; }
        /// <summary>
        /// 标识列(计算负载均衡用,计算zoneIndex,hashcode的列名）
        /// </summary>
        public string IdentityColumn { get; set; }
        /// <summary>
        /// 应用标识列
        /// </summary>
        public string AppIdentity { get; set; }
        /// <summary>
        /// 是否同步启动同步锁(解决客户端重复提交问题)
        /// </summary>
        public bool IsEnableLock { get; set; }
        /// <summary>
        /// 锁定用提供标识的列名(用于生成锁的唯一标识)
        /// </summary>
        public string[] LockColumn { get; set; }

        /// <summary>
        /// 是否即刻重置锁状态
        /// </summary>
        public RemoteLockType LockType { get; set; }

        /// <summary>
        /// 计算ZIndex的值
        /// </summary>
        public int[] ZIndexBit
        {
            get;
            set;
        }
        /// <summary>
        /// 负载均衡方案
        /// </summary>
        public string Transmit { get; set; }

    }
}
