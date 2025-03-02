using RpcCentral.Collect;
using RpcCentral.Collect.Model;
using RpcCentral.Service.Interface;
using WeDonekRpc.Model;

namespace RpcCentral.Service.RpcEvent.Event
{
    internal class RefreshCurVerNum : IRpcEvent
    {
        private readonly IRpcConfigCollect _Server;

        public RefreshCurVerNum (IRpcConfigCollect server)
        {
            this._Server = server;
        }

        public void Refresh (RpcTokenCache token, RefreshEventParam param)
        {
            long rpcMerId = long.Parse(param["RpcMerId"]);
            int verNum = int.Parse(param["VerNum"]);
            long systemTypeId = long.Parse(param["SystemTypeId"]);
            this._Server.RefreshVerNum(rpcMerId, systemTypeId, verNum);
        }
    }
}
