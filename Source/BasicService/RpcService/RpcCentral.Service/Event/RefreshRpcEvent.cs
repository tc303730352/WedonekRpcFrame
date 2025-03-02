using RpcCentral.Service.Interface;
using WeDonekRpc.Model;

namespace RpcCentral.Service.Event
{
    internal class RefreshRpcEvent : Route.TcpRoute<RefreshRpc>
    {
        private IRpcEventService _Service;
        public RefreshRpcEvent(IRpcEventService service) : base()
        {
            _Service = service;
        }
        protected override void ExecAction(RefreshRpc param)
        {
            _Service.Execate(param);
        }
    }
}
