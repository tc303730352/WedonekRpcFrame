using System;
using System.Threading;

using RpcClient.Collect;
using RpcClient.Config;

using RpcModel;
using RpcModel.RemoteLock;

using RpcHelper;

namespace RpcClient
{
        public class RemoteLock : IDisposable
        {
                /// <summary>
                /// 申请远程同步锁
                /// </summary>
                /// <param name="identif">锁标识</param>
                /// <param name="timeout">超时时间</param>
                /// <returns>锁</returns>
                public static RemoteLock ApplyLock(string identif, int timeout)
                {
                        return Collect.SyncLockCollect.ApplyLock(identif, timeout, WebConfig.RpcConfig.LockOverTime);
                }
                /// <summary>
                /// 申请远程同步锁
                /// </summary>
                /// <param name="identif">锁标识</param>
                /// <param name="timeout">超时时间</param>
                /// <param name="overTime">过期时间</param>
                /// <returns>锁</returns>
                public static RemoteLock ApplyLock(string identif, int timeout, int overTime)
                {
                        return Collect.SyncLockCollect.ApplyLock(identif, timeout, overTime);
                }
                /// <summary>
                /// 申请远程同步锁
                /// </summary>
                /// <param name="identif">锁标识</param>
                /// <returns></returns>
                public static RemoteLock ApplyLock(string identif)
                {
                        return Collect.SyncLockCollect.ApplyLock(identif, WebConfig.RpcConfig.LockTimeOut, WebConfig.RpcConfig.LockOverTime);
                }
                /// <summary>
                /// 申请远程同步锁
                /// </summary>
                /// <param name="identif"></param>
                /// <param name="lockType"></param>
                /// <returns></returns>
                public static RemoteLock ApplyLock(string identif, RemoteLockType lockType)
                {
                        return Collect.SyncLockCollect.ApplyLock(identif, lockType);
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
                /// 同步完成
                /// </summary>
                private const long _Complate = 2;

                private readonly RemoteLockType _LockType = RemoteLockType.同步锁;
                private readonly bool _isReset = false;
                internal RemoteLock(string lockId, RemoteLockType lockType) : this(lockId, WebConfig.RpcConfig.LockTimeOut, WebConfig.RpcConfig.LockOverTime)
                {
                        this._isReset = lockType != RemoteLockType.同步锁;
                        this._LockType = lockType;
                }
                internal RemoteLock(string lockId, int timeout, int overTime)
                {
                        this.LockId = lockId;
                        this.TimeOut = timeout;
                        this._LockTimeOut = (timeout * 1000) + 200;
                        this.ValidTime = overTime;
                }
                public string LockId
                {
                        get;
                }
                internal int OverTime
                {
                        get;
                        private set;
                }

                public int TimeOut { get; }

                /// <summary>
                /// 锁超时
                /// </summary>
                private readonly int _LockTimeOut = 2300;

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
                public string Extend => this._Extend;
                internal long LockServerId => this._LockServerId;
                public string SessionId { get; private set; }
                private volatile bool _IsLocal = true;
                private volatile bool _IsRelease = false;
                internal bool IsRelease => this._IsRelease;
                public int ValidTime { get; private set; }

                /// <summary>
                /// 原子锁
                /// </summary>
                private readonly ManualResetEvent _Lock = new ManualResetEvent(false);
                private string _Extend = null;

                private long _LockServerId = 0;
                /// <summary>
                /// 将当前线程挂起
                /// </summary>
                /// <param name="time"></param>
                private void _WaitSync(int time)
                {
                        if (this._IsRelease)
                        {
                                return;
                        }
                        else if (!this._Lock.WaitOne(time))
                        {
                                if (SyncLockCollect.SyncLock(this, out time))
                                {
                                        this._WaitSync(time);
                                }
                        }
                }
                /// <summary>
                /// 获取锁
                /// </summary>
                /// <returns></returns>
                public bool GetLock()
                {
                        long ls = Interlocked.CompareExchange(ref this._LockStatus, _Conduct, _Wait);
                        //当前线程属于自由状态
                        if (ls == _Wait)
                        {
                                if (!SyncLockCollect.SyncLock(this, out ApplyLockRes res, out this._LockServerId))
                                {
                                        if (res == null)
                                        {
                                                return false;
                                        }
                                        else
                                        {
                                                this._IsLocal = res.LockServerId == RpcStateCollect.ServerConfig.ServerId;
                                                this.SessionId = res.SessionId;
                                                if (res.LockServerId != RpcStateCollect.ServerConfig.ServerId && this._LockType == RemoteLockType.排斥锁)
                                                {
                                                        this.SetError("rpc.lock.occupy");
                                                        return false;
                                                }
                                                else if (res.LockStatus == RemoteLockStatus.待同步)
                                                {
                                                        this._WaitSync((res.TimeOut * 1000) + 50);
                                                }
                                                return false;
                                        }
                                }
                                this.SessionId = res.SessionId;
                                return true;
                        }
                        else if (ls == _Complate)
                        {
                                return false;//同步已完成
                        }
                        else if (!this._Lock.WaitOne(this._LockTimeOut) && !this._IsRelease)
                        {
                                if (SyncLockCollect.SyncLock(this, out int time) && time > 0)
                                {
                                        this._WaitSync(time);
                                }
                        }
                        return false;
                }

                internal void SetError(string error)
                {
                        this.IsError = true;
                        this.Error = error;
                        if (this.Unlock(null, this.ValidTime))
                        {
                                Collect.SyncLockCollect.DropLock(this.LockId);
                        }
                }
                /// <summary>
                /// 释放本地锁
                /// </summary>
                internal bool Unlock(string extend, int overTime)
                {
                        if (Interlocked.CompareExchange(ref this._LockStatus, _Complate, _Conduct) == _Conduct)
                        {
                                this.OverTime = HeartbeatTimeHelper.HeartbeatTime + overTime;
                                this._Extend = extend;
                                this._IsRelease = true;
                                if (this._Lock.Set())
                                {
                                        this._Lock.Dispose();
                                }
                                return true;
                        }
                        return false;
                }
                /// <summary>
                /// 释放锁
                /// </summary>
                /// <param name="extend"></param>
                /// <param name="isReset"></param>
                public void Exit(string extend)
                {
                        if (this.Unlock(extend, this.ValidTime))
                        {
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
                public void Exit(string extend, bool isReset)
                {
                        if (this.Unlock(extend, this.ValidTime))
                        {
                                if (this._IsLocal)
                                {
                                        SyncLockCollect.ReleaseLock(this, extend, isReset);
                                }
                                if (isReset)
                                {
                                        Collect.SyncLockCollect.DropLock(this.LockId);
                                }
                        }
                }
                /// <summary>
                /// 释放资源
                /// </summary>
                public void Dispose()
                {
                        if (Interlocked.Read(ref this._LockStatus) == _Conduct)
                        {
                                this.Exit(null);
                        }
                }
        }
}
