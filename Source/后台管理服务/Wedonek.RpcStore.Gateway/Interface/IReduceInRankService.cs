using Wedonek.RpcStore.Service.Model;

namespace Wedonek.RpcStore.Gateway.Service
{
        public interface IReduceInRankService
        {
                void Drop(long id);
                ReduceInRankConfig Get(long rpcMerId, long serverId);
                void Sync(AddReduceInRank datum);
        }
}