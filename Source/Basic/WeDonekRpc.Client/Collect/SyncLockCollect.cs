using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using WeDonekRpc.Client.RpcSysEvent;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;
using WeDonekRpc.Model.RemoteLock;

namespace WeDonekRpc.Client.Collect
{
    /// <summary>
    /// 本地远程锁集合
    /// </summary>
    internal class SyncLockCollect
    {
        /// <summary>
        /// 成功消息
        /// </summary>
        private static readonly TcpRemoteReply _SuccessMsg = new TcpRemoteReply(new BasicRes());
        /// <summary>
        /// 错误消息
        /// </summary>
        private static readonly TcpRemoteReply _ErrorMsg = new TcpRemoteReply(new BasicRes("unlock.error"));
        /// <summary>
        /// 锁集合
        /// </summary>
        private static readonly ConcurrentDictionary<string, RemoteLock> _LockList = new ConcurrentDictionary<string, RemoteLock>();
        private static readonly Timer _ClearTimer;
        static SyncLockCollect ()
        {
            _ClearTimer = new Timer(_ClearLock, null, 1000, 1000);
            RemoteSysEvent.AddEvent<Unlock>("Rpc_Unlock", _RemoteUnlock);
        }
        /// <summary>
        /// 释放锁事件
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private static TcpRemoteReply _RemoteUnlock (Unlock obj, MsgSource source)
        {
            return Unlock(obj.LockId, obj.Result, obj.OverTime) ? _SuccessMsg : _ErrorMsg;
        }
        /// <summary>
        /// 清理本地锁
        /// </summary>
        private static void _ClearLock (object state)
        {
            if (_LockList.Count == 0)
            {
                return;
            }
            int time = HeartbeatTimeHelper.HeartbeatTime;
            //string[] keys = _LockList.Where(a => a.Value.IsRelease && a.Value.OverTime < time).Select(a => a.Key).ToArray();
            string[] keys = _LockList.Keys.ToArray();
            keys.ForEach(a =>
            {
                if (!_LockList.TryGetValue(a, out RemoteLock syncLock))
                {
                    return;
                }
                else if (syncLock.IsRelease && syncLock.OverTime < time)
                {
                    if (_LockList.TryRemove(a, out syncLock))
                    {
                        syncLock.Dispose();
                    }
                }
                else if (!syncLock.IsRelease && syncLock.NextRefreshTime <= time)
                {
                    syncLock.Renewal();
                }
            });
        }

        /// <summary>
        /// 申请一个锁
        /// </summary>
        /// <param name="identif">锁的唯一标识</param>
        /// <param name="lockType">锁类型</param>
        /// <returns>锁对象</returns>
        public static RemoteLock ApplyLock (string identif, RemoteLockType lockType)
        {
            string lockId = string.Join("_", identif, RpcStateCollect.ServerConfig.SystemType);
            if (!_LockList.TryGetValue(lockId, out RemoteLock obj))
            {
                obj = _LockList.GetOrAdd(lockId, new RemoteLock(lockId, lockType));
            }
            return obj;
        }
        /// <summary>
        /// 申请一个锁
        /// </summary>
        /// <param name="identif">锁的唯一标识</param>
        /// <param name="lockType">锁类型</param>
        /// <param name="timeout">锁超时时长(秒)</param>
        /// <param name="overTime">同步锁失效时长(秒)</param>
        /// <returns>锁对象</returns>
        public static RemoteLock ApplyLock (string identif, RemoteLockType lockType, int timeout, int overTime)
        {
            string lockId = string.Join("_", identif, RpcStateCollect.ServerConfig.SystemType);
            if (!_LockList.TryGetValue(lockId, out RemoteLock obj))
            {
                obj = _LockList.GetOrAdd(lockId, new RemoteLock(lockId, lockType, timeout, overTime));
            }
            return obj;
        }
        /// <summary>
        /// 申请锁
        /// </summary>
        /// <param name="identif">锁的唯一标识</param>
        /// <param name="timeout">锁定超时时长(秒)</param>
        /// <param name="overTime">同步锁失效时长(秒)</param>
        /// <returns>锁对象</returns>
        public static RemoteLock ApplyLock (string identif, int timeout, int overTime)
        {
            string lockId = string.Join("_", identif, RpcStateCollect.ServerConfig.SystemType);
            if (_LockList.TryGetValue(lockId, out RemoteLock obj))
            {
                return obj;
            }
            else
            {
                return _LockList.GetOrAdd(lockId, new RemoteLock(lockId, timeout, overTime));
            }
        }
        /// <summary>
        /// 申请锁
        /// </summary>
        /// <param name="identif">锁的唯一标识</param>
        /// <param name="timeout">锁定超时时长(秒)</param>
        /// <param name="overTime">同步锁失效时长(秒)</param>
        /// <param name="isReset">是否即刻释放锁</param>
        /// <returns>锁对象</returns>
        public static RemoteLock ApplyLock (string identif, int timeout, int overTime, bool isReset)
        {
            string lockId = string.Join("_", identif, RpcStateCollect.ServerConfig.SystemType);
            if (!_LockList.TryGetValue(lockId, out RemoteLock obj))
            {
                obj = _LockList.GetOrAdd(lockId, new RemoteLock(lockId, timeout, overTime, isReset));
            }
            return obj;
        }
        /// <summary>
        /// 释放锁
        /// </summary>
        /// <param name="lockId">锁ID</param>
        /// <param name="result">执行结果</param>
        /// <param name="overTime">释放超时时间(秒)</param>
        /// <returns></returns>
        public static bool Unlock (string lockId, ExecResult result, int overTime)
        {
            if (_LockList.TryGetValue(lockId, out RemoteLock obj))
            {
                if (result.IsError)
                {
                    obj.SetError(result.ErrorMsg);
                }
                else
                {
                    obj.Exit(result, overTime);
                }
                return true;
            }
            return false;
        }
        /// <summary>
        /// 锁失效时删除锁
        /// </summary>
        /// <param name="lockId"></param>
        public static void DropLock (string lockId)
        {
            if (_LockList.TryRemove(lockId, out RemoteLock obj))
            {
                obj.Dispose();
            }
        }
        /// <summary>
        /// 通知控制节点释放锁
        /// </summary>
        /// <param name="obj">锁</param>
        /// <param name="extend">扩展参数-执行结果</param>
        /// <param name="isReset">是否即刻释放锁</param>
        /// <exception cref="ErrorException"></exception>
        public static void ReleaseLock (RemoteLock obj, string extend, bool isReset)
        {
            ReleaseLock msg = new ReleaseLock
            {
                LockId = obj.LockId,
                Extend = extend,
                IsError = false,
                SessionId = obj.SessionId,
                IsReset = isReset
            };
            if (!RemoteCollect.Send(obj.LockServerId, msg, out string error))
            {
                throw new ErrorException(error);
            }
        }
        public static void ReleaseLock (RemoteLock obj, string err)
        {
            ReleaseLock msg = new ReleaseLock
            {
                LockId = obj.LockId,
                Error = err,
                SessionId = obj.SessionId,
                IsError = true
            };
            if (!RemoteCollect.Send(obj.LockServerId, msg, out string error))
            {
                throw new ErrorException(error);
            }
        }
        /// <summary>
        /// 和控制节点同步锁状态
        /// </summary>
        /// <param name="rLock">锁</param>
        /// <param name="res">结果</param>
        /// <param name="serverId">锁的拥有者</param>
        /// <returns>是否成功</returns>
        internal static bool SyncLock (RemoteLock rLock, out ApplyLockRes res, out long serverId)
        {
            ApplyLock apply = new ApplyLock
            {
                LockId = rLock.LockId,
                ValidTime = rLock.ValidTime,
                LockTimeOut = rLock.TimeOut
            };
            if (!RemoteCollect.Send(apply, out res, out serverId, out string error))
            {
                rLock.SetError(error);
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
                    rLock.Exit(res.Result, res.OverTime);
                }
                return false;
            }
            return true;

        }
        /// <summary>
        /// 获取控制器上的锁状态
        /// </summary>
        /// <param name="obj">本地锁</param>
        /// <param name="time">锁超时时间(秒)</param>
        /// <returns></returns>
        public static bool GetLockStatus (RemoteLock obj, out int time)
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
                obj.Exit(res.Result, obj.OverTime);
                return false;
            }
            else
            {
                time = res.TimeOut > 0 ? ( res.TimeOut * 1000 ) + 10 : 10;
                return true;
            }
        }
        public static void CheckLockStatus (RemoteLock obj)
        {
            GetLockStatus msg = new GetLockStatus
            {
                LockId = obj.LockId,
                SessionId = obj.SessionId
            };
            if (!RemoteCollect.Send(obj.LockServerId, msg, out ApplyLockRes res, out string error))
            {
                obj.SetError(error);
            }
            else if (res.LockStatus == RemoteLockStatus.已释放)
            {
                obj.Exit(res.Result, obj.OverTime);
            }
        }
    }
}
