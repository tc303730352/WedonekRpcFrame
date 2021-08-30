using RpcModel;
using RpcModel.Server;

using RpcService.Collect;

namespace RpcService.Event
{
        internal class GetReduceInRankEvent : Route.TcpRoute<GetReduceInRank, ReduceInRank>
        {
                public GetReduceInRankEvent() : base("GetReduceInRank")
                {

                }
                protected override bool ExecAction(GetReduceInRank param, out ReduceInRank result, out string error)
                {
                        return ReduceInRankCollect.GetReduceInRank(param.RpcMerId, param.ServerId, out result, out error);
                }
        }
}
