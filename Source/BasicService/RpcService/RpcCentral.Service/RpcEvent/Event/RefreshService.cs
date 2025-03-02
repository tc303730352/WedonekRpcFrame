using RpcCentral.Collect;
using RpcCentral.Collect.Model;
using RpcCentral.Service.Interface;
using WeDonekRpc.Model;

namespace RpcCentral.Service.RpcEvent.Event
{
    internal class RefreshService : IRpcEvent
    {
        private readonly IRpcServerCollect _Server;
        private readonly IRpcServerConfigCollect _ServerConfig;
        public RefreshService(IRpcServerCollect server, IRpcServerConfigCollect serverConfig)
        {
            _Server = server;
            _ServerConfig = serverConfig;
        }
        public void Refresh(RpcTokenCache token, RefreshEventParam param)
        {
            string type = param["SystemType"];
            string val = param["Id"];
            if (string.IsNullOrEmpty(val))
            {
                return;
            }
            _Server.Refresh(long.Parse(val), param);
            _ServerConfig.Refresh(long.Parse(type));
        }
    }
}
