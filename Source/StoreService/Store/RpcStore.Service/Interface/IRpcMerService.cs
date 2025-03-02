using WeDonekRpc.Client;
using WeDonekRpc.Model;
using RpcStore.RemoteModel.Mer.Model;

namespace RpcStore.Service.Interface
{
    public interface IRpcMerService
    {
        long AddMer (RpcMerAdd mer);

        void Delete (long id);

        RpcMerDatum GetRpcMer (long id);

        BasicRpcMer[] GetBasic ();

        PagingResult<RpcMer> Query (string name, IBasicPage paging);

        void SetMer (long id, RpcMerSet datum);
    }
}