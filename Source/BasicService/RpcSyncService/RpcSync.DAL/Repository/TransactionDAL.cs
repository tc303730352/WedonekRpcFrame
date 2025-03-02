using RpcSync.Model;
using RpcSync.Model.DB;
using SqlSugar;
using WeDonekRpc.Model;
using WeDonekRpc.Model.Tran.Model;

namespace RpcSync.DAL.Repository
{
    internal class TransactionDAL : ITransactionDAL
    {
        public static readonly RpcTranMode[] _NeedSubmitMode = new RpcTranMode[]
            {
                RpcTranMode.Tcc,
                RpcTranMode.TwoPC
            };
        private readonly IRpcExtendResource<TransactionListModel> _BasicDAL;
        public TransactionDAL (IRpcExtendResource<TransactionListModel> dal)
        {
            this._BasicDAL = dal;
        }

        public long[] GetLockOverTimeTran ()
        {
            DateTime now = DateTime.Now.AddSeconds(10);
            ISugarQueryable<object>[] list = new ISugarQueryable<object>[]
            {
                this._BasicDAL.Queryable.Where(a =>
                a.TranStatus ==  TransactionStatus.待回滚 &&
                a.IsLock &&
                a.LockTime<=now).Select(a=>(object)new {
                    a.Id
                }),
               this._BasicDAL.Queryable.Where(a =>
              _NeedSubmitMode.Contains(a.TranMode) &&
               a.CommitStatus ==  TranCommitStatus.待提交 &&
               a.IsLock &&
               a.LockTime<=now).Select(a=>(object)new {
                    a.Id
                })
            };
            return this._BasicDAL.Gets<object, long>(list, "Id");
        }


        public TranLog[] GetTranLogs (long tranId)
        {
            return this._BasicDAL.Gets(c => c.ParentId == tranId && c.TranStatus == TransactionStatus.已提交, c => new TranLog
            {
                RpcMerId = c.RpcMerId,
                ServerId = c.ServerId,
                SystemType = c.SystemType,
                TranName = c.TranName
            });
        }
        /// <summary>
        /// 获取超时的事务
        /// </summary>
        /// <returns></returns>
        public long[] GetOverTimeTran ()
        {
            return this._BasicDAL.Gets(c => c.TranStatus == TransactionStatus.执行中 && c.IsRoot && c.OverTime <= DateTime.Now, c => c.Id);
        }

        public long CheckIsRepeat (long parentId, string dictate, MsgSource source)
        {
            return this._BasicDAL.Get(c => c.ParentId == parentId &&
            c.RpcMerId == source.RpcMerId &&
            c.SystemType == source.SystemType &&
            c.TranName == dictate, c => c.Id);
        }


        /// <summary>
        /// 获取需要重试的事务
        /// </summary>
        /// <returns></returns>
        public long[] GetRetryTran ()
        {
            DateTime now = DateTime.Now.AddSeconds(10);
            ISugarQueryable<object>[] list = new ISugarQueryable<object>[]
            {
                this._BasicDAL.Queryable.Where(a =>
                a.TranStatus ==  TransactionStatus.回滚失败 &&
                a.IsLock== false).Select(a=>(object)new {
                    a.Id
                }),
               this._BasicDAL.Queryable.Where(a =>
               _NeedSubmitMode.Contains(a.TranMode) &&
               a.CommitStatus ==  TranCommitStatus.提交失败 &&
               a.IsLock ==false).Select(a=>(object)new {
                    a.Id
                })
            };
            return this._BasicDAL.Gets<object, long>(list, "Id");
        }
        public long[] TranRollbackLock (long tranId)
        {
            long[] ids = this._BasicDAL.Gets(c => c.ParentId == tranId && c.IsLock == false, c => c.Id);
            if (ids.Length == 0)
            {
                return ids;
            }
            DateTime now = DateTime.Now;
            if (this._BasicDAL.Update(a => new TransactionListModel
            {
                IsLock = true,
                LockTime = now
            }, a => ids.Contains(a.Id, false) && a.IsLock == false))
            {
                return ids;
            }
            return null;
        }
        public long[] LockStayCommitTran (long tranId)
        {
            long[] ids = this._BasicDAL.Gets(c => c.ParentId == tranId
            && _NeedSubmitMode.Contains(c.TranMode)
            && c.CommitStatus == TranCommitStatus.待提交
            && c.IsLock == false, c => c.Id);
            if (ids.Length == 0)
            {
                return ids;
            }
            DateTime now = DateTime.Now;
            if (this._BasicDAL.Update(a => new TransactionListModel
            {
                IsLock = true,
                LockTime = now
            }, a => ids.Contains(a.Id, false) && a.IsLock == false))
            {
                return ids;
            }
            return null;
        }
        public bool SetTranExtend (long tranId, string extend)
        {
            return this._BasicDAL.Update(a => a.Extend == extend, a => a.Id == tranId && a.TranStatus == TransactionStatus.执行中);
        }

        public TransactionDatum GetTransaction (long id)
        {
            return this._BasicDAL.Get<TransactionDatum>(c => c.Id == id);
        }
        public RegTranState GetTranState (long id)
        {
            return this._BasicDAL.Get(c => c.Id == id, c => new RegTranState
            {
                AddTime = c.AddTime,
                Id = c.Id,
                IsLock = c.IsLock,
                OverTime = c.OverTime,
                TranStatus = c.TranStatus
            });
        }
        public void AddTran (TransactionListModel add)
        {
            this._BasicDAL.Insert(add);
        }


        public bool SubmitTran (long tranId)
        {
            return this._BasicDAL.Update()
                .SetColumns(a => a.TranStatus == TransactionStatus.已提交)
                .SetColumns(a => a.SubmitTime == DateTime.Now)
                .Where(c => c.ParentId == tranId && c.TranStatus == TransactionStatus.执行中)
                .ExecuteCommandHasChange();
        }

        public long[] RollbackTran (long[] tranId)
        {
            long[] ids = this._BasicDAL.Gets(c => tranId.Contains(c.ParentId)
            && c.TranStatus == TransactionStatus.执行中, c => c.Id);
            if (ids.Length == 0)
            {
                return ids;
            }
            DateTime now = DateTime.Now;
            if (this._BasicDAL.Update(a => new TransactionListModel
            {
                TranStatus = TransactionStatus.待回滚,
                IsLock = true,
                LockTime = now
            }, a => ids.Contains(a.Id, false) && a.IsLock == false))
            {
                return ids;
            }
            return null;
        }
        public bool RollbackTran (long tranId, TransactionStatus status)
        {
            return this._BasicDAL.Update()
          .SetColumns(a => a.TranStatus == TransactionStatus.待回滚)
          .SetColumns(a => a.FailTime == DateTime.Now)
          .Where(c => c.ParentId == tranId && c.TranStatus == status)
          .ExecuteCommandHasChange();
        }
        public bool RollbackSuccess (long tranId)
        {
            return this._BasicDAL.Update()
          .SetColumns(a => a.TranStatus == TransactionStatus.已回滚)
          .SetColumns(a => a.FailTime == DateTime.Now)
          .SetColumns(a => a.IsLock == false)
          .Where(c => c.Id == tranId)
          .ExecuteCommandHasChange();
        }
        public bool SetRollbackState (TransactionDatum tran)
        {
            return this._BasicDAL.Update()
              .SetColumns(a => a.TranStatus == tran.TranStatus)
              .SetColumns(a => a.RetryNum == tran.RetryNum)
              .SetColumns(a => a.Error == tran.Error)
               .SetColumns(a => a.IsLock == false)
              .Where(c => c.Id == tran.Id)
              .ExecuteCommandHasChange();
        }
        public bool CommitSuccess (long tranId)
        {
            return this._BasicDAL.Update()
              .SetColumns(a => a.CommitStatus == TranCommitStatus.已提交)
              .SetColumns(a => a.EndTime == DateTime.Now)
               .SetColumns(a => a.IsLock == false)
              .Where(c => c.Id == tranId)
              .ExecuteCommandHasChange();

        }
        public bool SetCommitState (TransactionDatum tran)
        {
            DateTime? end = tran.CommitStatus == TranCommitStatus.提交错误 ? DateTime.Now : null;
            return this._BasicDAL.Update()
              .SetColumns(a => a.CommitStatus == tran.CommitStatus)
              .SetColumns(a => a.RetryNum == tran.RetryNum)
              .SetColumns(a => a.Error == tran.Error)
              .SetColumns(a => a.EndTime == end)
               .SetColumns(a => a.IsLock == false)
              .Where(c => c.Id == tran.Id && c.IsLock)
              .ExecuteCommandHasChange();
        }
    }
}
