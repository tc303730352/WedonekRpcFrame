using RpcModel.Model;
using RpcModel.Server;

using RpcService.Logic;

namespace RpcService.Event
{
        internal class GetServerLimitEvent : Route.TcpRoute<GetServerLimit, LimitConfigRes>
        {
                public GetServerLimitEvent() : base("GetServerLimit")
                {

                }
                protected override bool ExecAction(GetServerLimit param, out LimitConfigRes result, out string error)
                {
                        return ServerLimitLogic.GetServerLimit(param, out result, out error);
                }
        }
}
