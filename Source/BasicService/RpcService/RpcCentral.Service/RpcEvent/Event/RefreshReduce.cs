using RpcCentral.Collect;
using RpcCentral.Collect.Model;
using RpcCentral.Service.Interface;
using WeDonekRpc.Model;

namespace RpcCentral.Service.RpcEvent.Event
{
    internal class RefreshReduce : IRpcEvent
    {
        private readonly IReduceInRankCollect _ReduceInRank;
        public RefreshReduce(IReduceInRankCollect reduceInRank)
        {
            _ReduceInRank = reduceInRank;
        }
        public void Refresh(RpcTokenCache token, RefreshEventParam param)
        {
            _ReduceInRank.Refresh(long.Parse(param["RpcMerId"]), long.Parse(param["ServerId"]));
        }
    }
}
