using WeDonekRpc.Model;
using WeDonekRpc.Model.Tran.Model;
using RpcSync.Model;
using RpcSync.Model.DB;

namespace RpcSync.DAL
{
    public interface ITransactionDAL
    {
        void AddTran(TransactionListModel add);
        long CheckIsRepeat(long parentId, string dictate, MsgSource source);
        bool CommitSuccess(long tranId);
        long[] GetLockOverTimeTran();
        long[] GetOverTimeTran();
        long[] GetRetryTran();
        TranLog[] GetTranLogs(long tranId);
        TransactionDatum GetTransaction(long id);
        RegTranState GetTranState(long id);
        long[] LockStayCommitTran(long tranId);
        bool RollbackSuccess(long tranId);
        bool RollbackTran(long tranId, TransactionStatus status);
        long[] RollbackTran(long[] tranId);
        bool SetCommitState(TransactionDatum tran);
        bool SetRollbackState(TransactionDatum tran);
        bool SetTranExtend(long tranId, string extend);
        bool SubmitTran(long tranId);
        long[] TranRollbackLock(long tranId);
    }
}