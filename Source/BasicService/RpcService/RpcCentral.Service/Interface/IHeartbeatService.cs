using WeDonekRpc.Model.Server;

namespace RpcCentral.Service.Interface
{
    public interface IHeartbeatService
    {
        int Heartbeat (ServiceHeartbeat obj, string conIp);
    }
}