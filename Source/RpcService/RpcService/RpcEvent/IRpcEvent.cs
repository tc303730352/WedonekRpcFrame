using RpcModel;

using RpcService.Model;

namespace RpcService.RpcEvent
{
        internal interface IRpcEvent
        {
                string EventKey { get; }
                void Refresh(RpcToken token, RefreshEventParam param);
        }
}