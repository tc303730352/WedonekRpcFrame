using WeDonekRpc.Client;
using WeDonekRpc.Client.Interface;
using RpcStore.RemoteModel.Tran;
using RpcStore.RemoteModel.Tran.Model;
using RpcStore.Service.Interface;

namespace RpcStore.Service.Event
{
    internal class TransactionEvent : IRpcApiService
    {
        private readonly ITransactionService _Service;
        private readonly ITransactionTreeService _TranTree;
        public TransactionEvent (ITransactionService service, ITransactionTreeService tranTree)
        {
            this._Service = service;
            this._TranTree = tranTree;
        }

        public TransactionData GetTransaction (GetTransaction obj)
        {
            return this._Service.Get(obj.TranId);
        }


        public PagingResult<TransactionLog> QueryTran (QueryTran query)
        {
            return this._Service.QueryRoot(query.Query, query.ToBasicPage());
        }

        public TransactionTree[] GetTranTree (GetTranTree obj)
        {
            return this._TranTree.GetTree(obj.TranId);
        }
    }
}
