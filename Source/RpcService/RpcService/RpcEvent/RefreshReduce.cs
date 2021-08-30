using RpcModel;

using RpcService.Collect;
using RpcService.Model;

namespace RpcService.RpcEvent
{
        internal class RefreshReduce : IRpcEvent
        {
                public string EventKey { get; } = "RefreshReduce";

                public void Refresh(RpcToken token, RefreshEventParam param)
                {
                        ReduceInRankCollect.Refresh(long.Parse(param["RpcMerId"]), long.Parse(param["ServerId"]));
                }
        }
}
