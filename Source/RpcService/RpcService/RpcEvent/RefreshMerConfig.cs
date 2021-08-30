using RpcModel;

using RpcService.Collect;
using RpcService.Model;

namespace RpcService.RpcEvent
{
        internal class RefreshMerConfig : IRpcEvent
        {
                public string EventKey { get; } = "RefreshMerConfig";

                public void Refresh(RpcToken token, RefreshEventParam param)
                {
                        long rpcMerId = long.Parse(param["RpcMerId"]);
                        long sysTypeId = long.Parse(param["SystemTypeId"]);
                        RpcServerConfigCollect.Refresh(rpcMerId, sysTypeId);
                }

        }
}
