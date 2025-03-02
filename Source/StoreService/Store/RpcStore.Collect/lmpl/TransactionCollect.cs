using WeDonekRpc.Helper;
using WeDonekRpc.Model;
using RpcStore.DAL;
using RpcStore.Model.ExtendDB;
using RpcStore.Model.Tran;
using RpcStore.RemoteModel.Tran.Model;

namespace RpcStore.Collect.lmpl
{

    internal class TransactionCollect : ITransactionCollect
    {
        private readonly ITransactionDAL _BasicDAL;

        public TransactionCollect (ITransactionDAL basicDAL)
        {
            this._BasicDAL = basicDAL;
        }

        public BasicTransaction[] QueryRoot (TransactionQuery query, IBasicPage paging, out int count)
        {
            return this._BasicDAL.QueryRoot(query, paging, out count);
        }

        public BasicTransaction[] GetTranNode (long rootId)
        {
            return this._BasicDAL.GetTranNode(rootId);
        }
        public TransactionListModel Get (long id)
        {
            TransactionListModel tran = this._BasicDAL.Get(id);
            if (tran == null)
            {
                throw new ErrorException("rpc.store.transaction.not.find");
            }
            return tran;
        }

        public BasicTransaction[] GetTranNodes (long[] rootId)
        {
            return this._BasicDAL.GetTranNodes(rootId);
        }
    }
}
