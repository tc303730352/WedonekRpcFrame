using RpcCentral.Model;

namespace RpcCentral.Collect
{
    public interface IServerGroupCollect
    {
        void ClearCache(long merId, long systemTypeId);
        RemoteConfig[] GetRemoteServer(long merId, long systemTypeId);
        long[] GetRemoteServerId(long merId, long systemTypeId);
        long[] GetRpcMer(long systemTypeId);
    }
}