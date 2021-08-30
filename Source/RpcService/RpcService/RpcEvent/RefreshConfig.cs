using RpcModel;

using RpcService.Collect;
using RpcService.Model;

namespace RpcService.RpcEvent
{
        internal class RefreshConfig : IRpcEvent
        {
                public string EventKey { get; } = "RefreshConfig";

                public void Refresh(RpcToken token, RefreshEventParam param)
                {
                        long type = long.Parse(param["SystemType"]);
                        RpcServerConfigCollect.Refresh(type);
                }

        }
}
