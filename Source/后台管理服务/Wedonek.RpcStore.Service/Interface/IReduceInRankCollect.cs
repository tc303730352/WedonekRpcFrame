using Wedonek.RpcStore.Service.Model;

namespace Wedonek.RpcStore.Service.Collect
{
        public interface IReduceInRankCollect
        {
                void DropReduceInRank(long id);
                ReduceInRankConfig GetReduceInRank(long rpcMerId, long serverId);
                long GetReduceInRankId(long rpcMerId, long serverId);
                void SyncReduceInRank(AddReduceInRank datum);
        }
}