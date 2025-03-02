using RpcCentral.Collect;
using RpcCentral.Collect.Model;
using RpcCentral.Service.Interface;
using WeDonekRpc.Model;

namespace RpcCentral.Service.RpcEvent.Event
{
    internal class RefreshVerConfig : IRpcEvent
    {
        private readonly IVerConfigCollect _VerConfig;
        public RefreshVerConfig (IVerConfigCollect verConfig)
        {
            this._VerConfig = verConfig;
        }

        public void Refresh (RpcTokenCache token, RefreshEventParam param)
        {
            long rpcMerId = long.Parse(param["RpcMerId"]);
            long sysTypeId = long.Parse(param["SystemTypeId"]);
            int ver = int.Parse(param["VerNum"]);
            this._VerConfig.Refresh(rpcMerId, sysTypeId, ver);
        }

    }
}
