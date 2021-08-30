using RpcModel;

using RpcService.Collect;
using RpcService.Model;

namespace RpcService.RpcEvent
{
        internal class RefreshService : IRpcEvent
        {
                public string EventKey { get; } = "RefreshService";

                public void Refresh(RpcToken token, RefreshEventParam param)
                {
                        string type = param["SystemType"];
                        string val = param["Id"];
                        if (string.IsNullOrEmpty(val))
                        {
                                return;
                        }
                        RpcServerCollect.Refresh(long.Parse(val), param);
                        RpcServerConfigCollect.Refresh(long.Parse(type));
                }
        }
}
