using System.Collections.Concurrent;
using System.Linq;

using RpcModel.RemoteLock;

using RpcHelper;

namespace RpcSyncService.Controller
{
        internal class SyncLockController
        {
                public SyncLockController(string lockId)
                {
                        this.LockId = lockId;
                }

                private readonly ConcurrentDictionary<string, LockController> _LockSession = new ConcurrentDictionary<string, LockController>();

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
                private string _CurrentId = null;
                private readonly LockHelper _Lock = new LockHelper();
                #region 申请锁
                public ApplyLockRes ApplyLock(ApplyLock apply, long serverId)
                {
                        LockController rlock = this._ApplyLock(apply);
                        if (rlock == null)
                        {
                                throw new ErrorException("lock.apply.fail");
                        }
                        return rlock.ApplyLock(serverId);
                }
                private LockController _ApplyLock(ApplyLock apply)
                {
                        if (this._CurrentId != null && this._LockSession.TryGetValue(this._CurrentId, out LockController rlock))
                        {
                                return rlock;
                        }
                        else if (this._Lock.GetLock())
                        {
                                if (this._CurrentId == null)
                                {
                                        rlock = new LockController(apply);
                                        if (this._LockSession.TryAdd(rlock.SessionId, rlock))
                                        {
                                                this._CurrentId = rlock.SessionId;
                                                this._Lock.Exit();
                                                return rlock;
                                        }
                                }
                                this._Lock.Exit();
                                return null;
                        }
                        else
                        {
                                return null;
                        }
                }

                #endregion

                internal ApplyLockRes GetLockStatus(GetLockStatus obj)
                {
                        if (this._LockSession.TryGetValue(obj.SessionId, out LockController rlock))
                        {
                                return rlock.GetLockStatus();
                        }
                        throw new ErrorException("lock.session.not.find");
                }

                public void ReleaseLock(ReleaseLock obj, long serverId)
                {
                        if (this._LockSession.TryGetValue(obj.SessionId, out LockController rlock))
                        {
                                rlock.ReleaseLock(serverId, obj.Extend);
                                return;
                        }
                        throw new ErrorException("lock.session.not.find");
                }

                public bool CheckLock()
                {
                        if (this._LockSession.Count > 0)
                        {
                                int time = HeartbeatTimeHelper.HeartbeatTime;
                                string[] keys = this._LockSession.Keys.ToArray();
                                keys.ForEach(a =>
                                {
                                        if (this._LockSession.TryGetValue(a, out LockController rlock))
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
                                                        this._CurrentId = null;
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
