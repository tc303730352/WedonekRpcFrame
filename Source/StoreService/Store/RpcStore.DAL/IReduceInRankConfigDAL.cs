using RpcStore.Model.DB;
using RpcStore.RemoteModel.ReduceInRank.Model;

namespace RpcStore.DAL
{
    public interface IReduceInRankConfigDAL
    {
        long Add (ReduceInRankConfigModel add);
        void Clear (long serverId);
        void Delete (long id);
        void Clear (long rpcMerId, long serverId);
        ReduceInRankConfigModel Get (long rpcMerId, long serverId);
        ReduceInRankConfigModel Get (long id);
        void Set (long id, ReduceInRankDatum datum);
    }
}