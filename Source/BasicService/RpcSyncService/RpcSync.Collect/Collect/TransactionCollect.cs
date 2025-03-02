using RpcSync.DAL;
using RpcSync.Model;
using RpcSync.Model.DB;
using WeDonekRpc.CacheClient.Interface;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.IdGenerator;
using WeDonekRpc.Model;
using WeDonekRpc.Model.Tran;
using WeDonekRpc.Model.Tran.Model;

namespace RpcSync.Collect.Collect
{
    internal class TransactionCollect : ITransactionCollect
    {
        private readonly ITransactionDAL _TranDAL;
        private readonly ICacheController _Cache;
        public TransactionCollect (ITransactionDAL tranDAL, ICacheController cache)
        {
            this._Cache = cache;
            this._TranDAL = tranDAL;
        }
        public long[] GetLockOverTimeTran ()
        {
            return this._TranDAL.GetLockOverTimeTran();
        }


        public long[] GetRetryTran ()
        {
            return this._TranDAL.GetRetryTran();
        }

        public long[] GetOverTimeTran ()
        {
            return this._TranDAL.GetOverTimeTran();
        }
        public TranLog[] GetTranLogs (long tranId)
        {
            return this._TranDAL.GetTranLogs(tranId);
        }

        public void SetTranExtend (long tranId, string extend)
        {
            if (!this._TranDAL.SetTranExtend(tranId, extend))
            {
                throw new ErrorException("rpc.tran.set.error");
            }
        }
        public TransactionDatum GetTransaction (long id)
        {
            string key = "Tran_" + id.ToString();
            if (this._Cache.TryGet(key, out TransactionDatum datum))
            {
                return datum;
            }
            datum = this._TranDAL.GetTransaction(id);
            if (datum != null)
            {
                _ = this._Cache.Add(key, datum, new TimeSpan(0, 30, 0));
            }
            return datum;
        }
        /// <summary>
        /// 回滚事务返回受影响的事务ID
        /// </summary>
        /// <param name="tranId"></param>
        /// <returns></returns>
        public long[] TranRollbackLock (long tranId)
        {
            long[] ids = this._TranDAL.TranRollbackLock(tranId);
            this._RefreshState(tranId);
            return ids;
        }
        public long[] LockStayCommitTran (long tranId)
        {
            long[] ids = this._TranDAL.LockStayCommitTran(tranId);
            this._RefreshState(tranId);
            return ids;
        }

        public void RollbackResult (TransactionDatum tran)
        {
            if (tran.TranStatus == TransactionStatus.已回滚)
            {
                if (!this._TranDAL.RollbackSuccess(tran.Id))
                {
                    throw new ErrorException("rpc.tran.rollbackstate.set.fail");
                }
            }
            else if (!this._TranDAL.SetRollbackState(tran))
            {
                throw new ErrorException("rpc.tran.rollbackstate.set.fail");
            }
            this._RefreshState(tran.Id);
        }
        public void CommitResult (TransactionDatum tran)
        {
            if (tran.CommitStatus == TranCommitStatus.已提交)
            {
                if (!this._TranDAL.CommitSuccess(tran.Id))
                {
                    throw new ErrorException("rpc.tran.commit.state.set.fail");
                }
            }
            else if (!this._TranDAL.SetCommitState(tran))
            {
                throw new ErrorException("rpc.tran.commit.state.set.fail");
            }
            this._RefreshState(tran.Id);
        }

        public bool SetRollbackState (TransactionDatum tran)
        {
            return this._TranDAL.SetRollbackState(tran);
        }

        /// <summary>
        /// 提交事务
        /// </summary>
        /// <param name="tranId"></param>
        public void SubmitTran (long tranId)
        {
            if (!this._TranDAL.SubmitTran(tranId))
            {
                throw new ErrorException("rpc.tran.submit.error");
            }
            this._RefreshState(tranId);
        }

        public long[] RollbackTran (long[] tranId)
        {
            return this._TranDAL.RollbackTran(tranId);
        }
        public void Rollback (RegTranState tran)
        {
            if (!this._TranDAL.RollbackTran(tran.Id, tran.TranStatus))
            {
                throw new ErrorException("rpc.tran.rollback.error");
            }
            this._RefreshState(tran.Id);
        }

        public bool TryGetTranState (long id, out RegTranState tran)
        {
            string key = string.Concat("TranState_", id);
            if (this._Cache.TryGet(key, out tran))
            {
                return true;
            }
            tran = this._TranDAL.GetTranState(id);
            if (tran != null)
            {
                _ = this._Cache.Add(key, tran, new TimeSpan(0, 30, 0));
                return true;
            }
            return false;
        }
        public RegTranState GetTranState (long id)
        {
            string key = string.Concat("TranState_", id);
            if (this._Cache.TryGet(key, out RegTranState tran))
            {
                return tran;
            }
            tran = this._TranDAL.GetTranState(id);
            if (tran == null)
            {
                throw new ErrorException("rpc.tran.not.find");
            }
            else
            {
                _ = this._Cache.Add(key, tran, new TimeSpan(0, 30, 0));
                return tran;
            }
        }
        public void ApplyTransaction (MsgSource source, ApplyTran apply)
        {
            DateTime now = DateTime.Now;
            TransactionListModel tran = new TransactionListModel
            {
                TranName = apply.TranName,
                RpcMerId = source.RpcMerId,
                SubmitJson = apply.SubmitJson,
                IsRoot = true,
                AddTime = now,
                OverTime = now.AddSeconds(apply.TimeOut),
                ServerId = source.ServerId,
                SystemType = source.SystemType,
                TranMode = apply.TranMode,
                TranStatus = TransactionStatus.执行中,
                Id = apply.TranId,
                ParentId = apply.TranId,
                RegionId = source.RegionId
            };
            this._TranDAL.AddTran(tran);
            RegTranState state = new RegTranState
            {
                Id = tran.Id,
                AddTime = now,
                OverTime = tran.OverTime
            };
            string key = string.Concat("TranState_", tran.Id);
            _ = this._Cache.Add(key, state, new TimeSpan(0, 30, 0));
        }

        public long CheckIsRepeat (long tranId, TranLogDatum datum, MsgSource source)
        {
            return this._TranDAL.CheckIsRepeat(tranId, datum.Dictate, source);
        }

        public long AddTranLog (RegTranState tran, TranLogDatum datum, MsgSource source)
        {
            TransactionListModel log = new TransactionListModel
            {
                TranName = datum.Dictate,
                TranMode = datum.TranMode,
                RpcMerId = source.RpcMerId,
                IsRoot = false,
                RegionId = source.RegionId,
                AddTime = DateTime.Now,
                OverTime = tran.OverTime,
                ServerId = datum.ServerId,
                SubmitJson = datum.SubmitJson,
                SystemType = datum.SystemType,
                ParentId = tran.Id,
                TranStatus = tran.TranStatus,
                Id = IdentityHelper.CreateId()
            };
            this._TranDAL.AddTran(log);
            return log.Id;
        }
        private void _RefreshState (long tranId)
        {
            string key = "TranState_" + tranId.ToString();
            _ = this._Cache.Remove(key);
            key = "Tran_" + tranId.ToString();
            _ = this._Cache.Remove(key);
        }
    }
}
