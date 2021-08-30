using System;
using System.Collections.Concurrent;
using System.Linq;

using RpcModel.RemoteLock;

using RpcSyncService.Controller;

using RpcHelper;
using RpcHelper.TaskTools;

namespace RpcSyncService.Collect
{
        internal class SyncLockCollect
        {
                static SyncLockCollect()
                {
                        TaskManage.AddTask(new TaskHelper("清理锁!", new TimeSpan(0, 0, 1), new Action(_ClearLock)));
                }

                private static void _ClearLock()
                {
                        if (_LockList.Count == 0)
                        {
                                return;
                        }
                        string[] keys = _LockList.Keys.ToArray();
                        keys.ForEach(a =>
                        {
                                if (_LockList.TryGetValue(a, out SyncLockController syncLock))
                                {
                                        if (syncLock.CheckLock())
                                        {
                                                _LockList.TryRemove(a, out syncLock);
                                        }
                                }
                        });
                }

                private static readonly ConcurrentDictionary<string, SyncLockController> _LockList = new ConcurrentDictionary<string, SyncLockController>();

                private static SyncLockController _ApplyLock(string lockId)
                {
                        if (!_LockList.TryGetValue(lockId, out SyncLockController syncLock))
                        {
                                return _LockList.GetOrAdd(lockId, new SyncLockController(lockId));
                        }
                        return syncLock;
                }

                internal static ApplyLockRes GetLockStatus(GetLockStatus obj)
                {
                        if (!_LockList.TryGetValue(obj.LockId, out SyncLockController syncLock))
                        {
                                throw new ErrorException("lock.not.find");
                        }
                        return syncLock.GetLockStatus(obj);
                }

                public static ApplyLockRes ApplyLock(ApplyLock apply, long serverId)
                {
                        SyncLockController obj = _ApplyLock(apply.LockId);
                        return obj.ApplyLock(apply, serverId);
                }
                public static void ReleaseLock(ReleaseLock obj, long serverId)
                {
                        if (!_LockList.TryGetValue(obj.LockId, out SyncLockController syncLock))
                        {
                                throw new ErrorException("lock.not.find");
                        }
                        syncLock.ReleaseLock(obj, serverId);
                }
        }
}
