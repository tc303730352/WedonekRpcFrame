using RpcStore.RemoteModel.Tran.Model;

namespace RpcStore.Service.Interface
{
    public interface ITransactionTreeService
    {
        TransactionTree[] GetTree (long id);
    }
}