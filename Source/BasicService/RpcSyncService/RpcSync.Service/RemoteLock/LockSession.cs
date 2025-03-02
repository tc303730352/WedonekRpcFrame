using RpcSync.Service.Interface;
using WeDonekRpc.Client.Collect;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.IdGenerator;
using WeDonekRpc.Helper.Lock;
using WeDonekRpc.Helper.Log;
using WeDonekRpc.Model;
using WeDonekRpc.Model.RemoteLock;

namespace RpcSync.Service.RemoteLock
{
    internal class LockSession : IDisposable
    {
        private readonly IRemoteLockConfig _Config;
        private readonly int _LockTime;
        public LockSession ( ApplyLock apply, IRemoteLockConfig config )
        {
            this._Config = config;
            this._LockStatus = config.WaitLock;
            this.SessionId = IdentityHelper.CreateIdOrTempId();
            this.LockId = apply.LockId;
            this._LockTime = apply.LockTimeOut;
            this._LockTimeOut = HeartbeatTimeHelper.HeartbeatTime + apply.LockTimeOut;
            this.OverTime = HeartbeatTimeHelper.HeartbeatTime + apply.ValidTime;
        }
        #region 属性
        /// <summary>
        /// 锁的会话
        /// </summary>
        public long SessionId
        {
            get;
        }
        public string LockId
        {
            get;
        }
        /// <summary>
        /// 过期时间
        /// </summary>
        public int OverTime { get; private set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public int ReleaseTime { get; private set; }
        /// <summary>
        /// 锁超时时间
        /// </summary>
        private int _LockTimeOut;
        /// <summary>
        /// 执行结果(数据交换)
        /// </summary>
        private ExecResult _Result;

        /// <summary>
        /// 锁状态
        /// </summary>
        private int _LockStatus;
        /// <summary>
        /// 拥有锁的服务器ID
        /// </summary>
        private long _LockServerId = 0;
        /// <summary>
        /// 包含锁的服务器列表
        /// </summary>
        private readonly List<long> _BindId = [];
        /// <summary>
        /// 锁
        /// </summary>
        private readonly LockHelper _Lock = new LockHelper();

        public bool IsEnd
        {
            get;
            private set;
        }
        #endregion

        public ApplyLockRes ApplyLock ( long serverId )
        {
            int status = Interlocked.CompareExchange(ref this._LockStatus, this._Config.ObtainLock, this._Config.WaitLock);
            if ( status == this._Config.WaitLock )
            {
                _ = Interlocked.Exchange(ref this._LockServerId, serverId);
                return new ApplyLockRes
                {
                    LockStatus = RemoteLockStatus.已锁,
                    LockServerId = serverId,
                    SessionId = this.SessionId,
                    TimeOut = this._LockTimeOut
                };
            }
            else if ( status == this._Config.ObtainLock )
            {
                long lockServerId = Interlocked.Read(ref this._LockServerId);
                if ( lockServerId != serverId )
                {
                    if ( this._Lock.GetLock() )
                    {
                        this._BindId.Add(serverId);
                        this._Lock.Exit();
                    }
                    int time = this._LockTimeOut - HeartbeatTimeHelper.HeartbeatTime;
                    return new ApplyLockRes
                    {
                        LockStatus = RemoteLockStatus.待同步,
                        SessionId = this.SessionId,
                        LockServerId = lockServerId,
                        TimeOut = time <= 0 ? 1 : time
                    };
                }
                else
                {
                    return new ApplyLockRes
                    {
                        LockStatus = RemoteLockStatus.已锁,
                        LockServerId = lockServerId,
                        SessionId = this.SessionId
                    };
                }
            }
            else
            {
                return new ApplyLockRes
                {
                    LockStatus = RemoteLockStatus.已释放,
                    SessionId = this.SessionId,
                    Result = this._Result,
                    OverTime = this.OverTime - HeartbeatTimeHelper.HeartbeatTime
                };
            }
        }

        #region 释放锁
        public void ReleaseLock ( long serverId, ExecResult result )
        {
            if ( Interlocked.Read(ref this._LockServerId) != serverId )
            {
                throw new ErrorException("lock.not.ownership");
            }
            else if ( !this._ReleaseLock(result) )
            {
                throw new ErrorException("lock.status.error");
            }
        }
        private bool _ReleaseLock ( ExecResult result )
        {
            int status = Interlocked.CompareExchange(ref this._LockStatus, this._Config.ReleaseLock, this._Config.ObtainLock);
            if ( status == this._Config.ObtainLock )
            {
                int time = HeartbeatTimeHelper.HeartbeatTime + 5;
                this.ReleaseTime = time > this.OverTime ? time : this.OverTime + 2;
                this.IsEnd = true;
                this._Result = result;
                this._SendMsg(result);
                return true;
            }
            return status == this._Config.ReleaseLock;
        }
        #endregion

        #region 发送释放通知
        private void _SendMsg ( ExecResult res )
        {
            if ( this._BindId.Count == 0 )
            {
                return;
            }
            long[] ids = null;
            if ( this._Lock.GetLock() )
            {
                ids = this._BindId.Distinct().ToArray();
                this._BindId.Clear();
                this._Lock.Exit();
            }
            if ( ids.Length > 0 )
            {
                Unlock msg = new Unlock
                {
                    LockId = this.LockId,
                    OverTime = this.OverTime - HeartbeatTimeHelper.HeartbeatTime,
                    Result = res
                };
                if ( ids.Length == 1 )
                {
                    this._SendUnlockMsg(ids[0], msg);
                }
                else
                {
                    _ = Parallel.ForEach(ids, a =>
                    {
                        this._SendUnlockMsg(a, msg);
                    });
                }
            }
        }
        private void _SendUnlockMsg ( long serverId, Unlock msg )
        {
            if ( !RemoteCollect.Send<Unlock>(serverId, msg, out string error) )
            {
                new WarnLog(error, "解锁信息发送错误!")
                                {
                                        { "Id",this.SessionId},
                                        { "ServerId",serverId},
                                        { "Msg",msg}
                                }.Save();
            }
        }
        #endregion

        private bool SetError ( string error )
        {
            ExecResult obj = new ExecResult
            {
                IsError = true,
                ErrorMsg = error
            };
            return this._ReleaseLock(obj);
        }
        public bool CheckLock ( int time )
        {
            if ( this.IsEnd )
            {
                return true;
            }
            int status = Interlocked.CompareExchange(ref this._LockStatus, 0, 0);
            if ( status == this._Config.ObtainLock )
            {
                if ( this._LockTimeOut < time )
                {
                    return this.SetError("lock.timeout");
                }
                return false;
            }
            return false;
        }
        public ApplyLockRes GetLockStatus ()
        {
            int status = Interlocked.CompareExchange(ref this._LockStatus, 0, 0);
            if ( status == this._Config.ObtainLock )
            {
                int time = this._LockTimeOut - HeartbeatTimeHelper.HeartbeatTime;
                return new ApplyLockRes
                {
                    LockStatus = RemoteLockStatus.待同步,
                    SessionId = this.SessionId,
                    LockServerId = Interlocked.Read(ref this._LockServerId),
                    TimeOut = time <= 0 ? 1 : time
                };
            }
            else if ( status == this._Config.ReleaseLock )
            {
                return new ApplyLockRes
                {
                    LockStatus = RemoteLockStatus.已释放,
                    SessionId = this.SessionId,
                    Result = this._Result
                };
            }
            else
            {
                throw new ErrorException("lock.status.error");
            }
        }

        public void Dispose ()
        {
            if ( !this.IsEnd )
            {
                _ = this.SetError("lock.dispose");
            }
            this._Lock.Dispose();
        }

        public void Renewal ( long serverId )
        {
            if ( Interlocked.Read(ref this._LockServerId) != serverId )
            {
                throw new ErrorException("lock.not.ownership");
            }
            int status = Interlocked.CompareExchange(ref this._LockStatus, this._Config.ObtainLock, this._Config.ObtainLock);
            this._LockTimeOut = status == this._Config.ObtainLock
                ? HeartbeatTimeHelper.HeartbeatTime + this._LockTime
                : throw new ErrorException("lock.status.change");
        }
    }
}
