using RpcCentral.Service.Interface;
using WeDonekRpc.Model.Model;
using WeDonekRpc.Model.Server;

namespace RpcCentral.Service.Event
{
    internal class GetServerLimitEvent : Route.TcpRoute<GetServerLimit, LimitConfigRes>
    {
        private IServerLimitService _ServerLimit;
        public GetServerLimitEvent(IServerLimitService serverLimit) : base()
        {
            _ServerLimit = serverLimit;
        }
        protected override LimitConfigRes ExecAction(GetServerLimit param)
        {
            return _ServerLimit.GetServerLimit(param);
        }
    }
}
