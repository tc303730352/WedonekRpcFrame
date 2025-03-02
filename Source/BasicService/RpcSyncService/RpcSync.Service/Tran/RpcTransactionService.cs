using WeDonekRpc.CacheClient.Interface;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;
using WeDonekRpc.Model.Tran;
using WeDonekRpc.Model.Tran.Model;
using RpcSync.Collect;
using RpcSync.Model;
using RpcSync.Service.Interface;

namespace RpcSync.Service.Tran
{
    internal class RpcTransactionService : IRpcTransactionService
    {
        private readonly ITransactionCollect _Transaction;
        private readonly ICacheController _Cache;
        private readonly IRollbackTranService _Rollback;
        private ICommitTranService _CommitTran;
        private ISingleTranService _SingleTran;
        public RpcTransactionService(ITransactionCollect transaction,
            ICommitTranService tranService,
            ISingleTranService singleTran,
            ICacheController cache,
            IRollbackTranService rollbackTran)
        {
            this._Cache = cache;
            this._CommitTran = tranService;
            this._SingleTran = singleTran;
            this._Rollback = rollbackTran;
            this._Transaction = transaction;
        }
        public void ApplyTransaction(MsgSource source, ApplyTran apply)
        {
            if (this._Transaction.TryGetTranState(apply.TranId, out RegTranState tran))
            {
                tran.CheckState();
            }
            this._Transaction.ApplyTransaction(source, apply);
        }
        public  void TranLockOverTime()
        {
            long[] tranId = this._Transaction.GetLockOverTimeTran();
            if (!tranId.IsNull())
            {
                this._SingleTran.AddQueue(tranId);
            }
        }
        /// <summary>
        /// 重启回滚失败的事务
        /// </summary>
        public void RestartRetryTran()
        {
            long[] tranId = this._Transaction.GetRetryTran();
            if (!tranId.IsNull())
            {
                this._SingleTran.AddQueue(tranId);
            }
        }
        /// <summary>
        /// 回滚事务
        /// </summary>
        /// <param name="tranId"></param>
        public void RollbackTran(long tranId)
        {
            RegTranState tran = _Transaction.GetTranState(tranId);
            tran.CheckState();
            this._Transaction.Rollback(tran);
            this._Rollback.AddQueue(tran.Id);
        }

        public void SubmitTran(long tranId)
        {
            RegTranState tran = this._Transaction.GetTranState(tranId);
            tran.CheckState();
            if (tran.TranStatus == TransactionStatus.已提交)
            {
                return;
            }
            this._Transaction.SubmitTran(tran.Id);
            this._CommitTran.AddQueue(tranId);
        }
        /// <summary>
        /// 检查超时的事务
        /// </summary>
        public void CheckOverTimeTran()
        {
            long[] tranId = this._Transaction.GetOverTimeTran();
            if (!tranId.IsNull())
            {
                long[] ids = _Transaction.RollbackTran(tranId);
                if (ids.Length > 0)
                {
                    this._SingleTran.AddQueue(ids);
                }
            }
        }
        public TranResult GetTranResult(long tranId)
        {
            RegTranState state = this._Transaction.GetTranState(tranId);
            if (state.TranStatus != TransactionStatus.已提交)
            {
                return new TranResult
                {
                    TranStatus = state.TranStatus,
                    BeginTime = state.AddTime
                };
            }
            return new TranResult
            {
                TranStatus = state.TranStatus,
                BeginTime = state.AddTime,
                Logs = this._Transaction.GetTranLogs(tranId)
            };
        }
        public long AddTranLog(long tranId, TranLogDatum datum, MsgSource source)
        {
            RegTranState state = this._Transaction.GetTranState(tranId);
            state.CheckIsEnd();
            string key = string.Join("_", "tran", tranId, datum.Dictate, source.RpcMerId, source.SystemType);
            if(_Cache.TryGet(key,out long id))
            {
                return id;
            }
            id = this._Transaction.CheckIsRepeat(tranId, datum, source);
            if (id == 0)
            {
                id = this._Transaction.AddTranLog(state, datum, source);
                _Cache.Set(key, id, new TimeSpan(0, 10, 0));
            }
            return id;
        }
    }
}
