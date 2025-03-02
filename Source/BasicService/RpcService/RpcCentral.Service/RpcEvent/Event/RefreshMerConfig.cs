using RpcCentral.Collect;
using RpcCentral.Collect.Model;
using RpcCentral.Service.Interface;
using WeDonekRpc.Model;

namespace RpcCentral.Service.RpcEvent.Event
{
    internal class RefreshMerConfig : IRpcEvent
    {
        private readonly IRpcServerConfigCollect _ServerConfig;
        private readonly IRpcConfigCollect _RpcConfig;
        public RefreshMerConfig (IRpcServerConfigCollect serverConfig, IRpcConfigCollect rpcConfig)
        {
            this._ServerConfig = serverConfig;
            this._RpcConfig = rpcConfig;
        }

        public void Refresh (RpcTokenCache token, RefreshEventParam param)
        {
            long rpcMerId = long.Parse(param["RpcMerId"]);
            long sysTypeId = long.Parse(param["SystemTypeId"]);
            this._ServerConfig.Refresh(rpcMerId, sysTypeId);
            this._RpcConfig.Refresh(rpcMerId, sysTypeId);
        }

    }
}
