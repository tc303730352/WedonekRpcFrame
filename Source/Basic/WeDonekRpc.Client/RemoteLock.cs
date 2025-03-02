using System;
using System.Threading;
using WeDonekRpc.Client.Collect;
using WeDonekRpc.Client.Config;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;
using WeDonekRpc.Model.RemoteLock;

namespace WeDonekRpc.Client
{
    /// <summary>
    /// 远程锁代理对象
    /// </summary>
    public class RemoteLock : IDisposable
    {
        /// <summary>
        /// 申请远程同步锁
        /// </summary>
        /// <param name="identif">锁标识</param>
        /// <param name="timeout">超时时间</param>
        /// <returns>锁</returns>
        public static RemoteLock ApplyLock (string identif, int timeout)
        {
            return Collect.SyncLockCollect.ApplyLock(identif, timeout, WebConfig.RpcConfig.LockValidTime);
        }
        /// <summary>
        /// 申请远程同步锁
        /// </summary>
        /// <param name="identif">锁标识</param>
        /// <param name="timeout">超时时间</param>
        /// <param name="overTime">过期时间</param>
        /// <returns>锁</returns>
        public static RemoteLock ApplyLock (string identif, int timeout, int overTime)
        {
            return Collect.SyncLockCollect.ApplyLock(identif, timeout, overTime);
        }
        public static RemoteLock ApplyLock (string identif, int timeout, int overTime, bool isReset)
        {
            return Collect.SyncLockCollect.ApplyLock(identif, timeout, overTime, isReset);
        }
        /// <summary>
        /// 申请远程同步锁
        /// </summary>
        /// <param name="identif">锁标识</param>
        /// <returns></returns>
        public static RemoteLock ApplyLock (string identif)
        {
            return Collect.SyncLockCollect.ApplyLock(identif, WebConfig.RpcConfig.LockTimeOut, WebConfig.RpcConfig.LockValidTime);
        }
        /// <summary>
        /// 申请远程同步锁
        /// </summary>
        /// <param name="identif"></param>
        /// <param name="lockType"></param>
        /// <returns></returns>
        public static RemoteLock ApplyLock (string identif, RemoteLockType lockType)
        {
            return Collect.SyncLockCollect.ApplyLock(identif, lockType);
        }
        /// <summary>
        /// 申请远程同步锁
        /// </summary>
        /// <param name="identif">锁标识</param>
        /// <param name="lockType">所类型</param>
        /// <param name="timeout">超时时间(秒)</param>
        /// <param name="overTime">过期时间(秒)</param>
        /// <returns></returns>
        public static RemoteLock ApplyLock (string identif, RemoteLockType lockType, int timeout, int overTime)
        {
            return Collect.SyncLockCollect.ApplyLock(identif, lockType, timeout, overTime);
        }
        /// <summary>
        /// 等待同步状态
        /// </summary>
        private const long _Wait = 0;

        /// <summary>
        /// 同步中
        /// </summary>
        private const long _Conduct = 1;
        /// <summary>
        /// 同步中
        /// </summary>
        private const long _Locked = 3;
        /// <summary>
        /// 同步完成
        /// </summary>
        private const long _Complate = 2;

        private readonly RemoteLockType _LockType = RemoteLockType.同步锁;
        private bool _isReset = false;
        internal RemoteLock (string lockId, RemoteLockType lockType) : this(lockId, WebConfig.RpcConfig.LockTimeOut, WebConfig.RpcConfig.LockValidTime)
        {
            this._isReset = lockType != RemoteLockType.同步锁;
            this._LockType = lockType;
        }
        internal RemoteLock (string lockId, RemoteLockType lockType, int timeout, int overTime) : this(lockId, timeout, overTime)
        {
            this._isReset = lockType != RemoteLockType.同步锁;
            this._LockType = lockType;
        }
        internal RemoteLock (string lockId, int timeout, int overTime)
        {
            this.LockId = lockId;
            this.TimeOut = timeout;
            this._LockTimeOut = timeout * 1000;
            this.NextRefreshTime = HeartbeatTimeHelper.HeartbeatTime + ( timeout / 3 );
            this.ValidTime = overTime;
        }
        internal RemoteLock (string lockId, int timeout, int overTime, bool isReset) : this(lockId, timeout, overTime)
        {
            this._isReset = isReset;
        }
        /// <summary>
        /// 下次刷新时间
        /// </summary>
        internal int NextRefreshTime = 0;
        /// <summary>
        /// 锁ID
        /// </summary>
        public string LockId
        {
            get;
        }
        /// <summary>
        /// 锁定超时时间
        /// </summary>
        internal int OverTime
        {
            get;
            private set;
        }
        /// <summary>
        /// 争抢锁的超时时间
        /// </summary>
        public int TimeOut { get; private set; }

        /// <summary>
        /// 锁超时
        /// </summary>
        private int _LockTimeOut = 10000;

        /// <summary>
        /// 琐的状态
        /// </summary>
        private long _LockStatus = _Wait;
        /// <summary>
        /// 是否错误
        /// </summary>
        public bool IsError
        {
            get;
            private set;
        }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string Error
        {
            get;
            private set;
        }
        /// <summary>
        /// 扩展参数
        /// </summary>
        public string Extend { get; private set; } = null;
        internal long LockServerId => this._LockServerId;
        /// <summary>
        /// 会话ID
        /// </summary>
        public long SessionId { get; private set; }

        private volatile bool _IsLocal = false;

        private volatile bool _IsRelease = false;
        /// <summary>
        /// 是否已释放
        /// </summary>
        internal bool IsRelease => this._IsRelease;
        /// <summary>
        /// 有效时间
        /// </summary>
        public int ValidTime { get; set; }

        /// <summary>
        /// 原子锁
        /// </summary>
        private readonly ManualResetEvent _Lock = new ManualResetEvent(false);
        private long _LockServerId;
        /// <summary>
        /// 将当前线程挂起
        /// </summary>
        /// <param name="time"></param>
        private void _WaitSync (int time)
        {
            if (this._IsRelease)
            {
                return;
            }
            this._LockTimeOut = time * 1000;
            if (!this._Lock.WaitOne(this._LockTimeOut))
            {
                if (SyncLockCollect.GetLockStatus(this, out time))
                {
                    this._WaitSync(time);
                }
            }
        }
        /// <summary>
        /// 获取锁
        /// </summary>
        /// <returns></returns>
        public bool GetLock ()
        {
            long ls = Interlocked.CompareExchange(ref this._LockStatus, _Conduct, _Wait);
            //当前线程属于自由状态
            if (ls == _Wait)
            {
                if (SyncLockCollect.SyncLock(this, out ApplyLockRes res, out this._LockServerId))
                {
                    this._IsLocal = res.LockServerId == RpcStateCollect.ServerConfig.ServerId;
                    this.SessionId = res.SessionId;
                    ls = Interlocked.CompareExchange(ref this._LockStatus, _Locked, _Conduct);
                    if (ls == _Conduct)
                    {
                        if (this._IsLocal == false && this._LockType == RemoteLockType.排斥锁)
                        {
                            this.SetError("rpc.lock.occupy");
                            return false;
                        }
                        else if (res.LockStatus == RemoteLockStatus.待同步)
                        {
                            this._WaitSync(res.TimeOut);
                            return false;
                        }
                        return true;
                    }
                }
                return false;
            }
            else if (ls == _Complate)
            {
                return false;//同步已完成
            }
            else if (!this._IsRelease && !this._Lock.WaitOne(this._LockTimeOut))
            {
                if (SyncLockCollect.GetLockStatus(this, out int time) && time > 0)
                {
                    this._WaitSync(time);
                }
            }
            return false;
        }
        /// <summary>
        /// 锁续期
        /// </summary>
        public void Renewal ()
        {
            if (this._IsLocal && Interlocked.Read(ref this._LockStatus) == _Locked)
            {
                if (this._LockServerId == 0)
                {

                }
                this.NextRefreshTime = HeartbeatTimeHelper.HeartbeatTime + ( this.TimeOut / 3 );
                LockRenewal msg = new LockRenewal
                {
                    LockId = this.LockId,
                    SessionId = this.SessionId
                };
                if (!RemoteCollect.Send(this._LockServerId, msg, out string error))
                {
                    if (error == "lock.status.change")
                    {
                        SyncLockCollect.CheckLockStatus(this);
                    }
                    else if (error.StartsWith("lock."))
                    {
                        this.SetError(error);
                    }
                }
            }
        }
        internal void SetError (string error)
        {
            if (!this._IsRelease)
            {
                this._IsRelease = true;
                this.IsError = true;
                this.Error = error;
                SyncLockCollect.DropLock(this.LockId);
                this._Unlock();
            }
        }
        /// <summary>
        /// 退出锁并通知其它线程错误
        /// </summary>
        /// <param name="error"></param>
        public void ExitError (string error)
        {
            if (!this._IsRelease)
            {
                this._IsRelease = true;
                this.IsError = true;
                this.Error = error;
                this._Unlock();
                SyncLockCollect.DropLock(this.LockId);
                if (this._IsLocal)
                {
                    SyncLockCollect.ReleaseLock(this, error);
                }
            }
        }
        /// <summary>
        /// 释放本地锁（远程通知释放）
        /// </summary>
        private void _Unlock ()
        {
            long ls = Interlocked.CompareExchange(ref this._LockStatus, _Complate, _Locked);
            if (ls == _Locked || ls == _Conduct)
            {
                if (this._Lock.Set())
                {
                    this._Lock.Dispose();
                }
            }
        }
        internal void Exit (ExecResult result, int overTime)
        {
            if (result.IsError)
            {
                this.SetError(result.ErrorMsg);
            }
            else if (!this._IsRelease)
            {
                this.Extend = result.Extend;
                this.OverTime = HeartbeatTimeHelper.HeartbeatTime + overTime;
                this._IsRelease = true;
                this._Unlock();
                if (result.IsReset)
                {
                    Collect.SyncLockCollect.DropLock(this.LockId);
                }
            }
        }
        /// <summary>
        /// 释放锁
        /// </summary>
        /// <param name="extend"></param>
        public void Exit (string extend)
        {
            if (!this._IsRelease)
            {
                this.OverTime = HeartbeatTimeHelper.HeartbeatTime + this.ValidTime;
                this._IsRelease = true;
                this.Extend = extend;
                this._Unlock();
                if (this._IsLocal)
                {
                    SyncLockCollect.ReleaseLock(this, extend, this._isReset);
                }
                if (this._isReset)
                {
                    Collect.SyncLockCollect.DropLock(this.LockId);
                }
            }
        }
        /// <summary>
        /// 释放锁
        /// </summary>
        public void Exit ()
        {
            this.Exit(null);
        }
        public void Exit (string extend, bool isReset)
        {
            this._isReset = isReset;
            this.Exit(extend);
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose ()
        {
            long status = Interlocked.Read(ref this._LockStatus);
            if (status == _Locked || status == _Conduct)
            {
                this.ExitError("rpc.lock.exec.exception");
            }
        }
    }
}
