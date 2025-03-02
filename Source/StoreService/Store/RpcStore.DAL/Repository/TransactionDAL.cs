using RpcStore.Model.ExtendDB;
using RpcStore.Model.Tran;
using RpcStore.RemoteModel.Tran.Model;
using WeDonekRpc.Model;

namespace RpcStore.DAL.Repository
{
    internal class TransactionDAL : ITransactionDAL
    {
        private readonly IRpcExtendResource<TransactionListModel> _BasicDAL;
        public TransactionDAL (IRpcExtendResource<TransactionListModel> dal)
        {
            this._BasicDAL = dal;
        }

        public BasicTransaction[] QueryRoot (TransactionQuery query, IBasicPage paging, out int count)
        {
            paging.InitOrderBy("Id", true);
            return this._BasicDAL.Query<BasicTransaction>(query.ToWhere(), paging, out count);
        }

        public BasicTransaction[] GetTranNode (long rootId)
        {
            return this._BasicDAL.Gets<BasicTransaction>(c => c.ParentId == rootId && c.IsRoot == false);
        }

        public BasicTransaction[] GetTranNodes (long[] rootId)
        {
            return this._BasicDAL.Gets<BasicTransaction>(c => rootId.Contains(c.ParentId));
        }

        public TransactionListModel Get (long id)
        {
            return this._BasicDAL.Get(c => c.Id == id);
        }
    }
}
