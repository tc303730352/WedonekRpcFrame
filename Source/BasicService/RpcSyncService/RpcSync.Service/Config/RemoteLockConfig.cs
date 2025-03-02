using RpcSync.Service.Interface;
using WeDonekRpc.Client.Attr;

namespace RpcSync.Service.Config
{
    [ClassLifetimeAttr(ClassLifetimeType.SingleInstance)]
    internal class RemoteLockConfig : IRemoteLockConfig
    {
        /// <summary>
        /// 待获取锁
        /// </summary>
        public int WaitLock { get; } = 0;
        /// <summary>
        /// 获得锁
        /// </summary>
        public int ObtainLock { get; } = 1;

        /// <summary>
        /// 释放锁            
        /// </summary>
        public int ReleaseLock { get; } = 2;


    }
}
