using WeDonekRpc.Client;
using WeDonekRpc.Model;
using RpcStore.RemoteModel.Tran;
using RpcStore.RemoteModel.Tran.Model;
using Store.Gatewary.Modular.Interface;

namespace Store.Gatewary.Modular.Services
{
    internal class TranService : ITranService
    {
        public TransactionData Get (long id)
        {
            return new GetTransaction
            {
                TranId = id
            }.Send();
        }

        public PagingResult<TransactionLog> Query (TransactionQuery query, IBasicPage paging)
        {
            TransactionLog[] logs = new QueryTran
            {
                Query = query,
                Index = paging.Index,
                Size = paging.Size,
                NextId = paging.NextId,
                SortName = paging.SortName,
                IsDesc = paging.IsDesc
            }.Send(out int count);
            return new PagingResult<TransactionLog>(logs, count);
        }

        public TransactionTree[] GetTree (long id)
        {
            return new GetTranTree
            {
                TranId = id
            }.Send();
        }

    }
}
