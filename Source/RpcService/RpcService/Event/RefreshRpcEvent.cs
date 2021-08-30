using RpcModel;

using RpcService.Logic;

namespace RpcService.Event
{
        internal class RefreshRpcEvent : Route.TcpRoute<RefreshRpc>
        {
                public RefreshRpcEvent() : base("RefreshRpc")
                {

                }
                protected override bool ExecAction(RefreshRpc param, out string error)
                {
                        return RpcEventLogic.Execate(param, out error);
                }
        }
}
