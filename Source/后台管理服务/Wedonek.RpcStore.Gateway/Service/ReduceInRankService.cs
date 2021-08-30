using Wedonek.RpcStore.Service.Collect;
using Wedonek.RpcStore.Service.Model;

namespace Wedonek.RpcStore.Gateway.Service
{
        internal class ReduceInRankService : IReduceInRankService
        {
                private readonly IReduceInRankCollect _ReduceInRank = null;
                public ReduceInRankService(IReduceInRankCollect reduce)
                {
                        this._ReduceInRank = reduce;
                }

                public void Drop(long id)
                {
                        this._ReduceInRank.DropReduceInRank(id);
                }

                public ReduceInRankConfig Get(long rpcMerId, long serverId)
                {
                        return this._ReduceInRank.GetReduceInRank(rpcMerId, serverId);
                }


                public void Sync(AddReduceInRank datum)
                {
                        this._ReduceInRank.SyncReduceInRank(datum);
                }
        }
}
