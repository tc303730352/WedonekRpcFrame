using System.Collections.Concurrent;
using RpcSync.Service.Interface;
using WeDonekRpc.Helper;
using WeDonekRpc.Model.RemoteLock;

namespace RpcSync.Service.RemoteLock
{
    internal class LockController
    {
        private readonly IRemoteLockConfig _Config;
        public LockController (string lockId, IRemoteLockConfig config)
        {
            this._Config = config;
            this.LockId = lockId;
        }

        private readonly ConcurrentDictionary<long, LockSession> _LockSession = new ConcurrentDictionary<long, LockSession>();

        /// <summary>
        /// 锁Id
        /// </summary>
        public string LockId
        {
            get;
        }

        /// <summary>
        /// 当前会话
        /// </summary>
        private long _CurrentId = 0;
        #region 申请锁
        public ApplyLockRes ApplyLock (ApplyLock apply, long serverId)
        {
            LockSession rlock = this._ApplyLock(apply);
            if (rlock == null)
            {
                throw new ErrorException("lock.apply.fail");
            }
            return rlock.ApplyLock(serverId);
        }
        private LockSession _ApplyLock (ApplyLock apply)
        {
            if (this._CurrentId != 0 && this._LockSession.TryGetValue(this._CurrentId, out LockSession rlock))
            {
                return rlock;
            }
            else if (this._CurrentId == 0)
            {
                rlock = new LockSession(apply, this._Config);
                if (this._LockSession.TryAdd(rlock.SessionId, rlock))
                {
                    this._CurrentId = rlock.SessionId;
                    return rlock;
                }
                return null;
            }
            else
            {
                return null;
            }
        }

        #endregion

        internal ApplyLockRes GetLockStatus (GetLockStatus obj)
        {
            if (this._LockSession.TryGetValue(obj.SessionId, out LockSession rlock))
            {
                return rlock.GetLockStatus();
            }
            throw new ErrorException("lock.session.not.find");
        }
        public void LockRenewal (LockRenewal obj, long serverId)
        {
            if (!this._LockSession.TryGetValue(obj.SessionId, out LockSession rlock))
            {
                throw new ErrorException("lock.session.not.find");
            }
            rlock.Renewal(serverId);
        }
        public void ReleaseLock (ReleaseLock obj, long serverId)
        {
            if (this._LockSession.TryGetValue(obj.SessionId, out LockSession rlock))
            {
                rlock.ReleaseLock(serverId, new ExecResult
                {
                    ErrorMsg = obj.Error,
                    IsError = obj.IsError,
                    Extend = obj.Extend,
                    IsReset = obj.IsReset
                });
                return;
            }
            throw new ErrorException("lock.session.not.find");
        }

        public bool CheckLock ()
        {
            if (this._LockSession.Count > 0)
            {
                int time = HeartbeatTimeHelper.HeartbeatTime;
                long[] keys = this._LockSession.Keys.ToArray();
                keys.ForEach(a =>
                {
                    if (this._LockSession.TryGetValue(a, out LockSession rlock))
                    {
                        if (rlock.IsEnd && rlock.ReleaseTime < time)
                        {
                            if (this._LockSession.TryRemove(a, out rlock))
                            {
                                rlock.Dispose();
                            }
                        }
                        else if (rlock.CheckLock(time) && rlock.OverTime < time && this._CurrentId == rlock.SessionId)
                        {
                            this._CurrentId = 0;
                        }
                    }
                });
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
