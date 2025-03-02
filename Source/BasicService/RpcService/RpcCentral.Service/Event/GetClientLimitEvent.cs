using RpcCentral.Collect;
using WeDonekRpc.Model.Model;
using WeDonekRpc.Model.Server;

namespace RpcCentral.Service.Event
{
    internal class GetClientLimitEvent : Route.TcpRoute<GetClientLimit, ServerClientLimit>
    {
        private IServerClientLimitCollect _ClientLimit;
        public GetClientLimitEvent(IServerClientLimitCollect clientLimit) : base()
        {
            _ClientLimit = clientLimit;
        }
        protected override ServerClientLimit ExecAction(GetClientLimit param)
        {
            return _ClientLimit.GetClientLimit(param.RpcMerId, param.ServerId);
        }
    }
}
