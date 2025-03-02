using WeDonekRpc.Client;
using WeDonekRpc.Model;
using RpcStore.RemoteModel.Tran.Model;

namespace Store.Gatewary.Modular.Interface
{
    public interface ITranService
    {
        TransactionData Get (long id);
        TransactionTree[] GetTree (long id);
        PagingResult<TransactionLog> Query (TransactionQuery query, IBasicPage paging);

    }
}
