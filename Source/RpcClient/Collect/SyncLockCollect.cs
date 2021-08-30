using System;
using System.Collections.Concurrent;
using System.Linq;

using RpcClient.RpcSysEvent;

using RpcModel;
using RpcModel.RemoteLock;

using RpcHelper;
using RpcHelper.TaskTools;

namespace RpcClient.Collect
{
        internal class SyncLockCollect
        {
                private static readonly TcpRemoteReply _SuccessMsg = new TcpRemoteReply { MsgBody = Tools.Json(new BasicRes()) };
                private static readonly TcpRemoteReply _ErrorMsg = new TcpRemoteReply { MsgBody = Tools.Json(new BasicRes("unlock.error")) };
                static SyncLockCollect()
                {
                        TaskManage.AddTask(new TaskHelper("清理锁!", new TimeSpan(0, 0, 1), new Action(_ClearLock)));
                        RemoteSysEvent.AddEvent<Unlock>("Rpc_Unlock", _RemoteUnlock);
                }
                private static TcpRemoteReply _RemoteUnlock(Unlock obj)
                {
                        return Unlock(obj.LockId, obj.Result, obj.OverTime) ? _SuccessMsg : _ErrorMsg;
                }
                private static void _ClearLock()
                {
                        if (_LockList.Count == 0)
                        {
                                return;
                        }
                        int time = RpcHelper.HeartbeatTimeHelper.HeartbeatTime;
                        string[] keys = _LockList.Where(a => a.Value.IsRelease && a.Value.OverTime < time).Select(a => a.Key).ToArray();
                        if (keys.Length > 0)
                        {
                                Array.ForEach(keys, a =>
                                {
                                        if (_LockList.TryRemove(a, out RemoteLock syncLock))
                                        {
                                                syncLock.Dispose();
                                        }
                                });
                        }
                }
                private static readonly ConcurrentDictionary<string, RemoteLock> _LockList = new ConcurrentDictionary<string, RemoteLock>();
                public static RemoteLock ApplyLock(string identif, RemoteLockType lockType)
                {
                        string lockId = string.Join("_", identif, RpcStateCollect.ServerConfig.SystemType);
                        return _LockList.TryGetValue(lockId, out RemoteLock obj) ? obj : _LockList.GetOrAdd(lockId, new RemoteLock(lockId, lockType));
                }
                public static RemoteLock ApplyLock(string identif, int timeout, int overTime)
                {
                        string lockId = string.Join("_", identif, RpcStateCollect.ServerConfig.SystemType);
                        return _LockList.TryGetValue(lockId, out RemoteLock obj)
                                ? obj
                                : _LockList.GetOrAdd(lockId, new RemoteLock(lockId, timeout, overTime));
                }
                public static bool Unlock(string lockId, ExecResult result, int overTime)
                {
                        if (_LockList.TryGetValue(lockId, out RemoteLock obj))
                        {
                                if (result.IsError)
                                {
                                        obj.SetError(result.ErrorMsg);
                                }
                                else
                                {
                                        return obj.Unlock(result.Extend, overTime);
                                }
                        }
                        return false;
                }
                public static void DropLock(string lockId)
                {
                        if (_LockList.TryRemove(lockId, out RemoteLock obj))
                        {
                                obj.Dispose();
                        }
                }
                public static void ReleaseLock(RemoteLock obj, string extend, bool isReset)
                {
                        ReleaseLock msg = new ReleaseLock
                        {
                                LockId = obj.LockId,
                                Extend = extend,
                                SessionId = obj.SessionId,
                                IsReset = isReset
                        };
                        if (!RemoteCollect.Send(obj.LockServerId, msg, out string error))
                        {
                                throw new ErrorException(error);
                        }
                }

                internal static bool SyncLock(RemoteLock rLock, out ApplyLockRes res, out long serverId)
                {
                        ApplyLock apply = new ApplyLock
                        {
                                LockId = rLock.LockId,
                                ValidTime = rLock.ValidTime,
                                LockTimeOut = rLock.TimeOut
                        };
                        if (!RemoteCollect.Send(apply, out res, out serverId, out string error))
                        {
                                res = null;
                                rLock.SetError(error);
                                return false;
                        }
                        else if (res.LockStatus == RemoteLockStatus.待同步)
                        {
                                return false;
                        }
                        else if (res.LockStatus == RemoteLockStatus.已释放)
                        {
                                if (res.Result.IsError)
                                {
                                        rLock.SetError(res.Result.ErrorMsg);
                                }
                                else
                                {
                                        rLock.Unlock(res.Result.Extend, res.OverTime);
                                }
                                return false;
                        }
                        return true;

                }

                public static bool SyncLock(RemoteLock obj, out int time)
                {
                        GetLockStatus msg = new GetLockStatus
                        {
                                LockId = obj.LockId,
                                SessionId = obj.SessionId
                        };
                        time = 0;
                        if (!RemoteCollect.Send(obj.LockServerId, msg, out ApplyLockRes res, out string error))
                        {
                                obj.SetError(error);
                                return false;
                        }
                        else if (res.LockStatus == RemoteLockStatus.已释放)
                        {
                                obj.Unlock(obj.Extend, obj.OverTime);
                                return false;
                        }
                        else
                        {
                                time = res.TimeOut > 0 ? (res.TimeOut * 1000) + 10 : 10;
                                return true;
                        }
                }
        }
}
