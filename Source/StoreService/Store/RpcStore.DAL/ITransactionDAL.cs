using WeDonekRpc.Model;
using RpcStore.Model.ExtendDB;
using RpcStore.Model.Tran;
using RpcStore.RemoteModel.Tran.Model;

namespace RpcStore.DAL
{
    public interface ITransactionDAL
    {
        TransactionListModel Get (long id);
        BasicTransaction[] GetTranNode (long rootId);
        BasicTransaction[] GetTranNodes (long[] rootId);
        BasicTransaction[] QueryRoot (TransactionQuery query, IBasicPage paging, out int count);
    }
}