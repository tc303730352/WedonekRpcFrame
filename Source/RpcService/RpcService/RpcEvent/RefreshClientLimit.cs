using RpcModel;

using RpcService.Collect;
using RpcService.Model;

namespace RpcService.RpcEvent
{
        internal class RefreshClientLimit : IRpcEvent
        {
                public string EventKey { get; } = "RefreshClientLimit";

                public void Refresh(RpcToken token, RefreshEventParam param)
                {
                        long rpcMerId = long.Parse(param["RpcMerId"]);
                        long serverId = long.Parse(param["ServerId"]);
                        ServerClientLimitCollect.Refresh(rpcMerId, serverId);
                }
        }
}
