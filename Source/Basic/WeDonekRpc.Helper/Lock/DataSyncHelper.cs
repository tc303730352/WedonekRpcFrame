using System.Threading;

namespace WeDonekRpc.Helper.Lock
{
    public class DataSyncHelper
    {
        /// <summary>
        /// 同步状态
        /// </summary>
        private long _SyncStatus = 0;
        /// <summary>
        /// 当前同步状态
        /// </summary>
        public long SyncStatus => Interlocked.Read(ref this._SyncStatus);
        /// <summary>
        /// 开始同步
        /// </summary>
        public const long SyncBegin = 1;
        /// <summary>
        /// 同步完成
        /// </summary>
        public const long SyncComplate = 2;
        /// <summary>
        /// 等待同步
        /// </summary>
        public const long WaitSync = 0;

        /// <summary>
        /// 开始同步
        /// </summary>
        /// <returns></returns>
        public bool BeginSync ()
        {
            return Interlocked.CompareExchange(ref this._SyncStatus, SyncBegin, WaitSync) == WaitSync;
        }
        /// <summary>
        /// 同步完成
        /// </summary>
        /// <returns></returns>
        public void EndSync ()
        {
            Interlocked.CompareExchange(ref this._SyncStatus, SyncComplate, SyncBegin);
        }
        /// <summary>
        /// 同步错误
        /// </summary>
        /// <returns></returns>
        public void SyncFail ()
        {
            Interlocked.CompareExchange(ref this._SyncStatus, WaitSync, SyncBegin);
        }
        /// <summary>
        /// 重置同步状态
        /// </summary>
        /// <param name="status">状态值</param>
        /// <param name="oldStatus">更改的状态</param>
        /// <returns>是否成功重置为新的状态</returns>
        public bool ResetSync ( long status, long oldStatus )
        {
            return Interlocked.CompareExchange(ref this._SyncStatus, status, oldStatus) == oldStatus;
        }
        public bool ResetSync ()
        {
            return Interlocked.CompareExchange(ref this._SyncStatus, WaitSync, SyncComplate) == SyncComplate;
        }

        public void SetSyncStatus ( long status )
        {
            Interlocked.Exchange(ref this._SyncStatus, status);
        }
    }
}
