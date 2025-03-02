using RpcCentral.Collect;
using RpcCentral.Collect.Model;
using RpcCentral.Service.Interface;
using WeDonekRpc.Model;

namespace RpcCentral.Service.RpcEvent.Event
{
    internal class RefreshMer : IRpcEvent
    {
        private readonly IRpcMerCollect _RpcMer;
        public RefreshMer(IRpcMerCollect rpcMer)
        {
            this._RpcMer = rpcMer;
        }

        public void Refresh(RpcTokenCache token, RefreshEventParam param)
        {
            long merId = long.Parse(param["RpcMerId"]);
            _RpcMer.Refresh(merId);
        }

    }
}
