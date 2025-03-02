using RpcCentral.Collect;
using RpcCentral.Collect.Model;
using RpcCentral.Service.Interface;
using WeDonekRpc.Model;

namespace RpcCentral.Service.RpcEvent.Event
{
    internal class RefreshConfig : IRpcEvent
    {
        private readonly IServerGroupCollect _ServerGroup;
        private readonly IRpcServerConfigCollect _ServerConfig;
        private readonly ITransmitConfigCollect _Transmit;
        public RefreshConfig(IServerGroupCollect serverGroup,
            IRpcServerConfigCollect serverConfig,
            ITransmitConfigCollect transmit)
        {
            _Transmit = transmit;
            _ServerConfig = serverConfig;
            _ServerGroup = serverGroup;
        }
        public void Refresh(RpcTokenCache token, RefreshEventParam param)
        {
            long type = long.Parse(param["SystemType"]);
            long rpcMerId = long.Parse(param["RpcMerId"]);
            _ServerGroup.ClearCache(rpcMerId, type);
            _ServerConfig.Refresh(type);
            _Transmit.Refresh(type, rpcMerId);
        }

    }
}
