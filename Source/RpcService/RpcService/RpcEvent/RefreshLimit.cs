using RpcModel;

using RpcService.Collect;
using RpcService.Model;

namespace RpcService.RpcEvent
{
        internal class RefreshLimit : IRpcEvent
        {
                public string EventKey { get; } = "RefreshLimit";

                public void Refresh(RpcToken token, RefreshEventParam param)
                {
                        long serverId = long.Parse(param["ServerId"]);
                        ServerLimitConfigCollect.Refresh(serverId);
                }

        }
}
