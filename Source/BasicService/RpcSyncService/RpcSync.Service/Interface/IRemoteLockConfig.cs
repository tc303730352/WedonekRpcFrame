namespace RpcSync.Service.Interface
{
    public interface IRemoteLockConfig
    {
        /// <summary>
        /// 已获取锁
        /// </summary>
        int ObtainLock { get; }
        /// <summary>
        /// 释放锁
        /// </summary>
        int ReleaseLock { get; }
        /// <summary>
        /// 待获取锁
        /// </summary>
        int WaitLock { get; }
    }
}