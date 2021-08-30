using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using RpcClient.Collect;

using RpcModel;
using RpcModel.RemoteLock;

using RpcSyncService.Config;

using RpcHelper;

namespace RpcSyncService.Controller
{
        internal class LockController : IDisposable
        {
                public LockController(ApplyLock apply)
                {
                        this.SessionId = Guid.NewGuid().ToString("N");
                        this.LockId = apply.LockId;
                        this._LockTimeOut = HeartbeatTimeHelper.HeartbeatTime + apply.LockTimeOut;
                        this.OverTime = HeartbeatTimeHelper.HeartbeatTime + apply.ValidTime;
                }
                #region 属性
                /// <summary>
                /// 锁的会话
                /// </summary>
                public string SessionId
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
                private readonly int _LockTimeOut;
                /// <summary>
                /// 执行结果(数据交换)
                /// </summary>
                private ExecResult _Result = null;

                /// <summary>
                /// 锁状态
                /// </summary>
                private int _LockStatus = SyncConfig.WaitLock;
                /// <summary>
                /// 拥有锁的服务器ID
                /// </summary>
                private long _LockServerId = 0;
                /// <summary>
                /// 包含锁的服务器列表
                /// </summary>
                private readonly List<long> _BindId = new List<long>();
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

                public ApplyLockRes ApplyLock(long serverId)
                {
                        int status = Interlocked.CompareExchange(ref this._LockStatus, SyncConfig.ObtainLock, SyncConfig.WaitLock);
                        if (status == SyncConfig.WaitLock)
                        {
                                Interlocked.Exchange(ref this._LockServerId, serverId);
                                return new ApplyLockRes
                                {
                                        LockStatus = RemoteLockStatus.已锁,
                                        LockServerId = serverId,
                                        SessionId = this.SessionId
                                };
                        }
                        else if (status == SyncConfig.ObtainLock)
                        {
                                long lockServerId = Interlocked.Read(ref this._LockServerId);
                                if (lockServerId != serverId)
                                {
                                        if (this._Lock.GetLock())
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
                                                TimeOut = time < 0 ? 0 : time
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
                                        Result = _Result,
                                        OverTime = this.OverTime - HeartbeatTimeHelper.HeartbeatTime
                                };
                        }
                }

                #region 释放锁
                public void ReleaseLock(long serverId, string extend)
                {
                        if (Interlocked.Read(ref this._LockServerId) != serverId)
                        {
                                throw new ErrorException("lock.not.ownership");
                        }
                        else if (!this._ReleaseLock(new ExecResult { Extend = extend }))
                        {
                                throw new ErrorException("lock.status.error");
                        }
                }
                private bool _ReleaseLock(ExecResult result)
                {
                        long status = Interlocked.CompareExchange(ref this._LockStatus, SyncConfig.ReleaseLock, SyncConfig.ObtainLock);
                        if (status == SyncConfig.ObtainLock)
                        {
                                int time = HeartbeatTimeHelper.HeartbeatTime + 5;
                                this.ReleaseTime = time > this.OverTime ? time : this.OverTime + 2;
                                this.IsEnd = true;
                                this._Result = result;
                                this._SendMsg(result);
                                return true;
                        }
                        return status == SyncConfig.ReleaseLock;
                }
                #endregion

                #region 发送释放通知
                private void _SendMsg(ExecResult res)
                {
                        if (this._BindId.Count == 0)
                        {
                                return;
                        }
                        long[] ids = null;
                        if (this._Lock.GetLock())
                        {
                                ids = this._BindId.Distinct().ToArray();
                                this._BindId.Clear();
                                this._Lock.Exit();
                        }
                        if (ids.Length > 0)
                        {
                                Unlock msg = new Unlock
                                {
                                        LockId = this.LockId,
                                        OverTime = this.OverTime - HeartbeatTimeHelper.HeartbeatTime,
                                        Result = res
                                };
                                if (ids.Length == 1)
                                {
                                        this._SendUnlockMsg(ids[0], msg);
                                }
                                else
                                {
                                        Parallel.ForEach(ids, a =>
                                        {
                                                this._SendUnlockMsg(a, msg);
                                        });
                                }
                        }
                }
                private void _SendUnlockMsg(long serverId, Unlock msg)
                {
                        if (!RemoteCollect.Send<Unlock>(serverId, msg, out string error))
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

                private bool SetError(string error)
                {
                        ExecResult obj = new ExecResult
                        {
                                IsError = true,
                                ErrorMsg = error
                        };
                        return this._ReleaseLock(obj);
                }
                public bool CheckLock(int time)
                {
                        if (this.IsEnd)
                        {
                                return true;
                        }
                        int status = Interlocked.CompareExchange(ref this._LockStatus, 0, 0);
                        if (status == SyncConfig.ObtainLock)
                        {
                                if (this._LockTimeOut < time)
                                {
                                        return this.SetError("lock.timeout");
                                }
                                return false;
                        }
                        return false;
                }
                public ApplyLockRes GetLockStatus()
                {
                        int status = Interlocked.CompareExchange(ref this._LockStatus, 0, 0);
                        if (status == SyncConfig.ObtainLock)
                        {
                                int time = this._LockTimeOut - HeartbeatTimeHelper.HeartbeatTime;
                                return new ApplyLockRes
                                {
                                        LockStatus = RemoteLockStatus.待同步,
                                        SessionId = this.SessionId,
                                        LockServerId = Interlocked.Read(ref this._LockServerId),
                                        TimeOut = time < 0 ? 0 : time
                                };
                        }
                        else if (status == SyncConfig.ReleaseLock)
                        {
                                return new ApplyLockRes
                                {
                                        LockStatus = RemoteLockStatus.已释放,
                                        SessionId = this.SessionId,
                                        Result = _Result
                                };
                        }
                        else
                        {
                                throw new ErrorException("lock.status.error");
                        }
                }

                public void Dispose()
                {
                        if (!this.IsEnd)
                        {
                                this.SetError("lock.dispose");
                        }
                        this._Lock.Dispose();
                }
        }
}
