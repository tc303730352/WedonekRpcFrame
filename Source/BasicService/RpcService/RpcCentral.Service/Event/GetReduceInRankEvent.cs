using RpcCentral.Collect;
using WeDonekRpc.Model.Server;

namespace RpcCentral.Service.Event
{
    internal class GetReduceInRankEvent : Route.TcpRoute<GetReduceInRank, ReduceInRank>
    {
        private IReduceInRankCollect _ReduceInRank;
        public GetReduceInRankEvent(IReduceInRankCollect reduceInRank):base()
        {
            _ReduceInRank = reduceInRank;
        }
        protected override ReduceInRank ExecAction(GetReduceInRank param)
        {
            return _ReduceInRank.GetReduceInRank(param.RpcMerId, param.ServerId);
        }
    }
}
