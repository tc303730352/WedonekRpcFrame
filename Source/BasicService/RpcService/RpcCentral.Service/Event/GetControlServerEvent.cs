using RpcCentral.Collect;
using WeDonekRpc.Model.Model;

namespace RpcCentral.Service.Event
{
    internal class GetControlServerEvent : Route.TcpRouteByResult<RpcControlServer[]>
    {
        private IRpcControlCollect _RpcControl;
        public GetControlServerEvent(IRpcControlCollect rpcControl) : base()
        {
            _RpcControl = rpcControl;
        }
        protected override RpcControlServer[] ExecAction()
        {
            return _RpcControl.GetControlServer();
        }
    }
}
