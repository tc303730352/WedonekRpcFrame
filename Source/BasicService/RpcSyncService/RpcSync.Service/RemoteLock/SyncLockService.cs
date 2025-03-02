using System.Collections.Concurrent;
using RpcSync.Service.Interface;
using WeDonekRpc.Client.Attr;
using WeDonekRpc.Helper;
using WeDonekRpc.Model.RemoteLock;

namespace RpcSync.Service.RemoteLock
{
    [ClassLifetimeAttr(ClassLifetimeType.SingleInstance)]
    internal class SyncLockService : ISyncLockService
    {
        private readonly Timer _ClearLockTimer;
        private readonly ConcurrentDictionary<string, LockController> _LockList = new ConcurrentDictionary<string, LockController>();
        private readonly IRemoteLockConfig _Config;
        public SyncLockService (IRemoteLockConfig config)
        {
            this._Config = config;
            this._ClearLockTimer = new Timer(new TimerCallback(this._ClearLock), null, 1000, 1000);
        }

        private void _ClearLock (object state)
        {
            if (this._LockList.Count == 0)
            {
                return;
            }
            string[] keys = this._LockList.Keys.ToArray();
            keys.ForEach(a =>
            {
                if (this._LockList.TryGetValue(a, out LockController syncLock))
                {
                    if (syncLock.CheckLock())
                    {
                        _ = this._LockList.TryRemove(a, out syncLock);
                    }
                }
            });
        }


        private LockController _ApplyLock (string lockId)
        {
            if (!this._LockList.TryGetValue(lockId, out LockController syncLock))
            {
                return this._LockList.GetOrAdd(lockId, new LockController(lockId, this._Config));
            }
            return syncLock;
        }

        public ApplyLockRes GetLockStatus (GetLockStatus obj)
        {
            if (!this._LockList.TryGetValue(obj.LockId, out LockController syncLock))
            {
                throw new ErrorException("lock.not.find");
            }
            return syncLock.GetLockStatus(obj);
        }
        public void LockRenewal (LockRenewal obj, long serverId)
        {
            if (!this._LockList.TryGetValue(obj.LockId, out LockController syncLock))
            {
                throw new ErrorException("lock.not.find");
            }
            syncLock.LockRenewal(obj, serverId);
        }

        public ApplyLockRes ApplyLock (ApplyLock apply, long serverId)
        {
            LockController obj = this._ApplyLock(apply.LockId);
            return obj.ApplyLock(apply, serverId);
        }
        public void ReleaseLock (ReleaseLock obj, long serverId)
        {
            if (!this._LockList.TryGetValue(obj.LockId, out LockController syncLock))
            {
                throw new ErrorException("lock.not.find");
            }
            syncLock.ReleaseLock(obj, serverId);
        }
    }
}
