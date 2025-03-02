using WeDonekRpc.Model.Server;

namespace RpcCentral.Service.Interface
{
    public interface IGetServerNodeService
    {
        ServerConfigInfo Get (long serverId, long sourceId);
    }
}