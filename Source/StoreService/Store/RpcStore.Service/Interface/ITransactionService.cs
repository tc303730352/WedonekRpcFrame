using WeDonekRpc.Client;
using WeDonekRpc.Model;
using RpcStore.RemoteModel.Tran.Model;

namespace RpcStore.Service.Interface
{
    public interface ITransactionService
    {
        TransactionData Get (long id);
        PagingResult<TransactionLog> QueryRoot (TransactionQuery query, IBasicPage paging);
    }
}