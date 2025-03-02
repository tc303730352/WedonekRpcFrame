using WeDonekRpc.Model;
using WeDonekRpc.Model.Tran;
using WeDonekRpc.Model.Tran.Model;
using RpcSync.Model;

namespace RpcSync.Collect
{
    public interface ITransactionCollect
    {
        void RollbackResult(TransactionDatum tran);
        void ApplyTransaction(MsgSource source, ApplyTran apply);
        bool TryGetTranState(long id,out RegTranState state);
        long AddTranLog(RegTranState tran, TranLogDatum datum, MsgSource source);
        long CheckIsRepeat(long tranId, TranLogDatum datum, MsgSource source);
        void CommitResult(TransactionDatum tran);
        long[] GetLockOverTimeTran();
        long[] GetOverTimeTran();
        long[] GetRetryTran();
        TranLog[] GetTranLogs(long tranId);
        TransactionDatum GetTransaction(long id);
        RegTranState GetTranState(long id);
        long[] LockStayCommitTran(long tranId);
        void Rollback(RegTranState tran);
        long[] RollbackTran(long[] tranId);
        bool SetRollbackState(TransactionDatum tran);
        void SetTranExtend(long tranId, string extend);
        void SubmitTran(long tranId);
        long[] TranRollbackLock(long tranId);
    }
}