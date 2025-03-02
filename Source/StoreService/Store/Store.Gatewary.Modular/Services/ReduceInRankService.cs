using RpcStore.RemoteModel.ReduceInRank;
using RpcStore.RemoteModel.ReduceInRank.Model;
using Store.Gatewary.Modular.Interface;

namespace Store.Gatewary.Modular.Services
{
    internal class ReduceInRankService : IReduceInRankService
    {
        public void DeleteReduceInRank (long id)
        {
            new DeleteReduceInRank
            {
                Id = id,
            }.Send();
        }

        public ReduceInRankConfig GetReduceInRank (long rpcMerId, long serverId)
        {
            return new GetReduceInRank
            {
                RpcMerId = rpcMerId,
                ServerId = serverId,
            }.Send();
        }

        public void SyncReduceInRank (ReduceInRankAdd datum)
        {
            new SyncReduceInRank
            {
                Datum = datum,
            }.Send();
        }

    }
}
