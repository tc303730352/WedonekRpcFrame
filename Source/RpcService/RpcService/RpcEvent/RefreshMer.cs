using RpcModel;

using RpcService.Collect;
using RpcService.Model;

using RpcHelper;

namespace RpcService.RpcEvent
{
        internal class RefreshMer : IRpcEvent
        {
                public string EventKey { get; } = "RefreshMer";

                public void Refresh(RpcToken token, RefreshEventParam param)
                {
                        long merId = long.Parse(param["RpcMerId"]);
                        if (!RpcMerCollect.Refresh(merId, out string error))
                        {
                                new WarnLog(error, "刷新集群信息错误!")
                                {
                                        LogContent = string.Concat("RpcMerId:", param["RpcMerId"])
                                }.Save();
                        }
                }

        }
}
