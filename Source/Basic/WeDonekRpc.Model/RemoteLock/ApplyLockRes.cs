namespace WeDonekRpc.Model.RemoteLock
{
    public class ApplyLockRes
    {
        /// <summary>
        /// 锁状态
        /// </summary>
        public RemoteLockStatus LockStatus
        {
            get;
            set;
        }
        /// <summary>
        /// 结果
        /// </summary>
        public ExecResult Result
        {
            get;
            set;
        }
        /// <summary>
        /// 申请到锁的服务节点ID
        /// </summary>
        public long LockServerId { get; set; }
        /// <summary>
        /// 锁定超时时间
        /// </summary>
        public int TimeOut { get; set; }
        /// <summary>
        /// 回话ID
        /// </summary>
        public long SessionId { get; set; }
        /// <summary>
        /// 锁释放超时时间
        /// </summary>
        public int OverTime { get; set; }
    }
}
