using RpcStore.Model.DB;
using RpcStore.RemoteModel.ReduceInRank.Model;

namespace RpcStore.Collect
{
    public interface IReduceInRankCollect
    {
        ReduceInRankConfigModel Get(long id);
        void Delete(ReduceInRankConfigModel obj);
        ReduceInRankConfigModel Get(long rpcMerId, long serverId);
        bool Sync(ReduceInRankAdd datum);
    }
}