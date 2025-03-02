using WeDonekRpc.Model;
using WeDonekRpc.Model.Tran;
using WeDonekRpc.Model.Tran.Model;

namespace RpcSync.Service.Interface
{
    public interface IRpcTransactionService
    {
        void TranLockOverTime();
        void RestartRetryTran();
        void SubmitTran(long tranId);
        TranResult GetTranResult(long tranId);
        void ApplyTransaction(MsgSource source, ApplyTran apply);
        void RollbackTran(long tranId);
        void CheckOverTimeTran();
        long AddTranLog(long tranId, TranLogDatum datum, MsgSource source);
    }
}