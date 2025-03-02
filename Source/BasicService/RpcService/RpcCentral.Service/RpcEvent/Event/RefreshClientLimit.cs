using RpcCentral.Collect;
using RpcCentral.Collect.Model;
using RpcCentral.Service.Interface;
using WeDonekRpc.Model;

namespace RpcCentral.Service.RpcEvent.Event
{
    internal class RefreshClientLimit : IRpcEvent
    {
        private readonly IServerClientLimitCollect _ClientLimit;
        public RefreshClientLimit(IServerClientLimitCollect clientLimit)
        {
            this._ClientLimit = clientLimit;
        }

        public void Refresh(RpcTokenCache token, RefreshEventParam param)
        {
            long rpcMerId = long.Parse(param["RpcMerId"]);
            long serverId = long.Parse(param["ServerId"]);
            _ClientLimit.Refresh(rpcMerId, serverId);
        }
    }
}
