using RpcCentral.Collect;
using RpcCentral.Collect.Model;
using RpcCentral.Service.Interface;
using WeDonekRpc.Model;

namespace RpcCentral.Service.RpcEvent.Event
{
    internal class RefreshTransmit : IRpcEvent
    {
        private readonly ITransmitConfigCollect _TransmitConfig;
        private readonly IServerGroupCollect _ServerGroup;
        public RefreshTransmit(ITransmitConfigCollect transmit,
            IServerGroupCollect serverGroup)
        {
            this._TransmitConfig = transmit;
            this._ServerGroup = serverGroup;
        }

        public void Refresh(RpcTokenCache token, RefreshEventParam param)
        {
            long rpcMerId = long.Parse(param["RpcMerId"]);
            long sysTypeId = long.Parse(param["SystemTypeId"]);
            _ServerGroup.ClearCache(rpcMerId, sysTypeId);
            _TransmitConfig.Refresh(sysTypeId, rpcMerId);
        }

    }
}
