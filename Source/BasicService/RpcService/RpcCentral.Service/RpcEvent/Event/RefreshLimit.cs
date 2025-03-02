using RpcCentral.Collect;
using RpcCentral.Collect.Model;
using RpcCentral.Service.Interface;
using WeDonekRpc.Model;

namespace RpcCentral.Service.RpcEvent.Event
{
    internal class RefreshLimit : IRpcEvent
    {
        private readonly IServerLimitConfigCollect _limitConfig;
        public RefreshLimit(IServerLimitConfigCollect limitConfig)
        {
            this._limitConfig = limitConfig;
        }

        public void Refresh(RpcTokenCache token, RefreshEventParam param)
        {
            _limitConfig.Refresh(long.Parse(param["ServerId"]));
        }

    }
}
