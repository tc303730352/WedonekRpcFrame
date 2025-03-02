using RpcCentral.Model;
using RpcCentral.Model.DB;

namespace RpcCentral.DAL
{
    public interface IRemoteServerGroupDAL
    {
        RemoteConfig[] GetRemoteServer (long rpcMerId, long systemTypeId);
        long[] GetRemoteServerId (long rpcMerId, long systemTypeId);
        long[] GetRpcMer (long systemTypeId);
        RemoteServerGroup Find (long rpcMerId, long serverId);
    }
}