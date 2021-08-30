using RpcModel.Model;
using RpcModel.Server;

using RpcService.Collect;

namespace RpcService.Event
{
        internal class GetClientLimitEvent : Route.TcpRoute<GetClientLimit, ServerClientLimit>
        {
                public GetClientLimitEvent() : base("GetClientLimit")
                {

                }
                protected override bool ExecAction(GetClientLimit param, out ServerClientLimit result, out string error)
                {
                        return ServerClientLimitCollect.GetClientLimit(param.RpcMerId, param.ServerId, out result, out error);
                }
        }
}
