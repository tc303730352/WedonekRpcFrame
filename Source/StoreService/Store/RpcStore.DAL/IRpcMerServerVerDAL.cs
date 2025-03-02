using RpcStore.Model.DB;

namespace RpcStore.DAL
{
    public interface IRpcMerServerVerDAL
    {
        int? GetVer (long rpcMerId, long systemTypeId);
        RpcMerServerVerModel Find (long rpcMerId, long systemTypeId);
        Dictionary<long, int> GetVers (long rpcMerId);
        void SetCurrentVer (long id, int ver);
        Dictionary<long, int> GetVers (long[] rpcMerId, long[] systemTypeId);
        void Add (RpcMerServerVerModel add);
    }
}