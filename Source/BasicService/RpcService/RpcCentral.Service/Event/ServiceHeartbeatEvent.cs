using RpcCentral.Service.Interface;
using RpcCentral.Service.Route;
using WeDonekRpc.Model.Server;
namespace RpcCentral.Service.Event
{
    internal class ServiceHeartbeatEvent : TcpRoute<ServiceHeartbeat, int>
    {
        private readonly IHeartbeatService _Service;
        public ServiceHeartbeatEvent (IHeartbeatService service) : base()
        {
            this._Service = service;
        }
        protected override int ExecAction (ServiceHeartbeat param, string clientIp)
        {
            return this._Service.Heartbeat(param, clientIp);
        }
    }
}
