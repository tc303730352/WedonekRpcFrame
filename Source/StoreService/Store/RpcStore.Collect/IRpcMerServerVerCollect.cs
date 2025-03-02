using RpcStore.Model.DB;

namespace RpcStore.Collect
{
    public interface IRpcMerServerVerCollect
    {
        void Add (long rpcMerId, long systemTypeId, int verNum);
        RpcMerServerVerModel Find (long rpcMerId, long systemTypeId);
        Dictionary<long, int> GetVers (long rpcMerId);
        Dictionary<long, int> GetVers (long[] rpcMerId, long[] systemTypeId);
        void SetCurrentVer (RpcMerServerVerModel source, int ver);
    }
}