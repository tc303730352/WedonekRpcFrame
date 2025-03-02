using RpcSync.Model;
using WeDonekRpc.Model;

namespace RpcSync.Collect
{
    public interface IRemoteServerGroupCollect
    {
        long[] GetHoldServerId (long merId);
        void Refresh (long merId, long[] remoteId);
        MerServer[] GetAllServer (long merId);

        string[] GetServerTypeVal (long merId);
        bool CheckIsExists (long rpcMerId, MsgSource source);
    }
}