using RpcStore.RemoteModel.ReduceInRank.Model;

namespace RpcStore.Service.Interface
{
    public interface IReduceInRankService
    {
        void Delete(long id);
        ReduceInRankConfig Get(long rpcMerId, long serverId);
        void Sync(ReduceInRankAdd add);
    }
}