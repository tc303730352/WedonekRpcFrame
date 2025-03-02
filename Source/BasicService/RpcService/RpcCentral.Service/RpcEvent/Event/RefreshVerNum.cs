using RpcCentral.Collect;
using RpcCentral.Collect.Model;
using RpcCentral.Service.Interface;
using WeDonekRpc.Model;

namespace RpcCentral.Service.RpcEvent.Event
{
    internal class RefreshVerNum : IRpcEvent
    {
        private readonly IRpcServerCollect _Server;

        public RefreshVerNum (IRpcServerCollect server)
        {
            this._Server = server;
        }

        public void Refresh (RpcTokenCache token, RefreshEventParam param)
        {
            long serverId = long.Parse(param["ServerId"]);
            int verNum = int.Parse(param["VerNum"]);
            int oldVerNum = int.Parse(param["OldVerNum"]);
            this._Server.RefreshVerNum(serverId, verNum, oldVerNum);
        }
    }
}
