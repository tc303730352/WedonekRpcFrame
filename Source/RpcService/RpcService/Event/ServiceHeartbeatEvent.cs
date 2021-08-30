using RpcModel.Server;

using RpcService.Logic;
using RpcService.Route;
namespace RpcService.Event
{
        internal class ServiceHeartbeatEvent : TcpRoute<ServiceHeartbeat>
        {
                public ServiceHeartbeatEvent() : base("ServiceHeartbeat")
                {

                }
                protected override bool ExecAction(ServiceHeartbeat param, string clientIp, out string error)
                {
                        return ServiceLogic.ServiceHeartbeat(param, clientIp, out error);
                }
        }
}
