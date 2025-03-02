using WeDonekRpc.Model;
using RpcStore.Model.DB;
using RpcStore.Model.RpcMer;
using RpcStore.RemoteModel.Mer.Model;

namespace RpcStore.Collect
{
    public interface IRpcMerCollect
    {
        void CheckAppId (string appId);
        long AddMer (RpcMerAdd mer);
        void CheckSystemName (string name);
        void Delete (RpcMerModel mer);
        Dictionary<long, string> GetNames (long[] ids);
        RpcMerModel GetRpcMer (long id);
        RpcMerModel[] Query (string queryKey, IBasicPage paging, out int count);
        bool SetMer (RpcMerModel mer, RpcMerSetDatum datum);
        BasicRpcMer[] GetBasic ();
        BasicRpcMer[] GetBasic (long[] rpcMerId);
        string GetName (long rpcMerId);
    }
}