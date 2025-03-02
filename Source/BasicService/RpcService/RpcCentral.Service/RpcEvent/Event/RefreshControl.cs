using RpcCentral.Collect;
using RpcCentral.Collect.Model;
using RpcCentral.Service.Interface;
using WeDonekRpc.Model;

namespace RpcCentral.Service.RpcEvent.Event
{
    internal class RefreshControl : IRpcEvent
    {
        private readonly IRpcControlCollect _Control;

        public RefreshControl(IRpcControlCollect rpcControl)
        {
            _Control = rpcControl;
        }

        public void Refresh(RpcTokenCache token, RefreshEventParam param)
        {
            _Control.Refresh();
        }

    }
}
