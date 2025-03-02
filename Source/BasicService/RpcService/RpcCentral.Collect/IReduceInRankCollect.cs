using WeDonekRpc.Model.Server;

namespace RpcCentral.Collect
{
    public interface IReduceInRankCollect
    {
        void Refresh(long rpcMerId, long serverId);
        ReduceInRank GetReduceInRank(long rpcMerId, long serverId);
    }
}