using RpcCentral.Model.DB;

namespace RpcCentral.DAL
{
    public interface IReduceInRankConfigDAL
    {
        ReduceInRankConfig GetReduceInRank(long rpcMerId, long servrId);
    }
}