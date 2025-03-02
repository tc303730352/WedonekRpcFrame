using WeDonekRpc.Model;
using RpcStore.Model.DB;
using RpcStore.Model.RpcMer;
using RpcStore.RemoteModel.Mer.Model;

namespace RpcStore.DAL
{
    public interface IRpcMerDAL
    {
        long Add (RpcMerModel add);
        bool CheckSystemName (string systemName);

        bool CheckAppId (string appId);
        void Delete (long id);
        RpcMerModel Get (long id);
        Dictionary<long, string> GetNames (long[] ids);
        RpcMerModel[] Query (string queryKey, IBasicPage paging, out int count);
        void Set (long id, RpcMerSetDatum datum);
        BasicRpcMer[] GetBasic ();
        BasicRpcMer[] GetBasic (long[] rpcMerId);
        string GetName (long rpcMerId);
    }
}