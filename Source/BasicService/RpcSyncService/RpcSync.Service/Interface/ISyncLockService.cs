using WeDonekRpc.Model.RemoteLock;

namespace RpcSync.Service.Interface
{
    public interface ISyncLockService
    {
        ApplyLockRes ApplyLock (ApplyLock apply, long serverId);
        ApplyLockRes GetLockStatus (GetLockStatus obj);
        void ReleaseLock (ReleaseLock obj, long serverId);
        void LockRenewal (LockRenewal obj, long serverId);
    }
}