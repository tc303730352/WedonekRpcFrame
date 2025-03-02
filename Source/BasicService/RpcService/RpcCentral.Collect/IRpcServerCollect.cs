using RpcCentral.Collect.Controller;
using RpcCentral.Model;
using RpcCentral.Model.DB;
using WeDonekRpc.Model;

namespace RpcCentral.Collect
{
    public interface IRpcServerCollect
    {
        ServerCont FindService (ContainerGetArg arg);
        RpcServerController FindRpcServer (long sysTypeId, string mac, int serverIndex);
        RpcServerController GetRpcServer (long serverId);
        void LoadServer ();
        void Refresh (long id, RefreshEventParam param);
        void RefreshVerNum (long serverId, int verNum, int oldVerNum);
        long AddContainer (RemoteServerConfig config, string sysType);
        void SetContainerId (long serverId, long containerId);
    }
}