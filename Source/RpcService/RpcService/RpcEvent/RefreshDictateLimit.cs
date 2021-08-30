using RpcModel;

using RpcService.Collect;
using RpcService.Model;

namespace RpcService.RpcEvent
{
        internal class RefreshDictateLimit : IRpcEvent
        {
                public string EventKey { get; } = "RefreshDictateLimit";

                public void Refresh(RpcToken token, RefreshEventParam param)
                {
                        long serverId = long.Parse(param["ServerId"]);
                        ServerDictateLimitCollect.Refresh(serverId);
                }

        }
}
