using RpcModel.Model;

using RpcService.Logic;

namespace RpcService.Event
{
        internal class GetControlServerEvent : Route.TcpRouteByResult<RpcControlServer[]>
        {
                public GetControlServerEvent() : base("GetControlServer")
                {

                }
                protected override bool ExecAction(out RpcControlServer[] result, out string error)
                {
                        return RpcControlLogic.GetControls(out result, out error);
                }
        }
}
