using RpcModel;

using RpcService.Logic;

namespace RpcService.Event
{
        internal class GetServerListEvent : Route.TcpRoute<GetServerList, GetServerListRes>
        {
                public GetServerListEvent() : base("GetServerList")
                {

                }
                protected override bool ExecAction(GetServerList param, out GetServerListRes result, out string error)
                {
                        return ServiceLogic.GetServerList(param, out result, out error);
                }
        }
}
