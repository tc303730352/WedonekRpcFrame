using RpcSync.Model;
using WeDonekRpc.Model;

namespace RpcSync.DAL
{
    public interface IRemoteServerGroupDAL
    {
        bool CheckIsExists (long rpcMerId, MsgSource source);
        MerServer[] GetAllServer (long merId);
        long[] GetHoldServerId (long merId);
        string[] GetServerTypeVal (long merId);
    }
}