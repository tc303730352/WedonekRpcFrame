using System;
using System.Data;

using RpcModel;
using RpcModel.Tran.Model;

using RpcSyncService.Model;

using SqlExecHelper;
using SqlExecHelper.SetColumn;

using RpcHelper;

namespace RpcSyncService.DAL
{
        internal class TransactionDAL : SqlBasicClass
        {
                public TransactionDAL() : base("TransactionList", "RpcExtendService")
                {

                }
                /// <summary>
                /// 获取挂起的事务
                /// </summary>
                /// <param name="ids"></param>
                /// <returns></returns>
                public bool GetHangUpTran(out Guid[] ids)
                {
                        DateTime now = DateTime.Now;
                        return this.Get("Id", out ids, new ISqlWhere[] {
                               new SqlWhere("TranStatus", SqlDbType.SmallInt){Value=TransactionStatus.已提交},
                               new SqlWhere("IsMainTran", SqlDbType.Bit){Value=1},
                               new SqlWhere("IsEnd", SqlDbType.Bit){Value=0},
                               new SqlWhere("OverTime", SqlDbType.SmallDateTime,QueryType.小等){ Value=now}
                        });
                }
                public bool GetLockOverTimeTran(out Guid[] ids)
                {
                        DateTime now = DateTime.Now.AddSeconds(10);
                        return this.Get("Id", out ids, new ISqlWhere[] {
                               new SqlWhere("TranStatus", SqlDbType.SmallInt){Value=TransactionStatus.待回滚},
                               new SqlWhere("IsLock", SqlDbType.Bit){Value=1},
                               new SqlWhere("FailTime", SqlDbType.SmallDateTime,QueryType.小等){ Value=now}
                        });
                }
                public bool GetTranState(Guid[] parentId, out Model.TranState[] states)
                {
                        return this.Get("ParentId", parentId, out states);
                }
                public TranLog[] GetTranLogs(Guid tranId)
                {
                        if (this.UnionQuery(out TranLog[] logs, new ISqlWhere[] {
                                new SqlWhere("MainTranId", SqlDbType.UniqueIdentifier){Value=tranId},
                                new SqlWhere("TranStatus", SqlDbType.SmallInt){Value=TransactionStatus.已提交},
                                new SqlWhere("IsMainTran", SqlDbType.Bit){Value=1}
                        }, new ISqlWhere[] {
                                new SqlWhere("ParentId", SqlDbType.UniqueIdentifier){Value=tranId},
                                new SqlWhere("TranStatus", SqlDbType.SmallInt){Value=TransactionStatus.已提交}
                        }))
                        {
                                return logs;
                        }
                        throw new ErrorException("rpc.tran.log.get.error");
                }
                /// <summary>
                /// 获取超时的事务
                /// </summary>
                /// <param name="trans"></param>
                /// <returns></returns>
                public bool GetOverTimeTran(out Guid[] tranId)
                {
                        DateTime now = DateTime.Now;
                        return this.Group("MainTranId", out tranId, new ISqlWhere[] {
                               new SqlWhere("TranStatus", SqlDbType.SmallInt){Value=TransactionStatus.执行中},
                               new SqlWhere("OverTime", SqlDbType.SmallDateTime,QueryType.小等){ Value=now}
                        });
                }

                public bool CheckIsRepeat(Guid parentId, string dictate, MsgSource source, out Guid id)
                {
                        return this.ExecuteScalar("Id", out id, new ISqlWhere[] {
                               new SqlWhere("ParentId", SqlDbType.UniqueIdentifier){Value=parentId},
                               new SqlWhere("RpcMerId", SqlDbType.BigInt){Value=source.RpcMerId},
                               new SqlWhere("SystemTypeId", SqlDbType.BigInt){ Value=source.SystemTypeId},
                               new SqlWhere("TranName", SqlDbType.VarChar,50){Value=dictate}
                        });
                }

                internal bool SetTranState(SetTranState[] list)
                {
                        using (IBatchUpdate update = this.BatchUpdate(list.Length, 5))
                        {
                                update.Column = new SqlTableColumn[]
                                {
                                        new SqlTableColumn("Id", SqlDbType.UniqueIdentifier){ ColType= SqlColType.查询},
                                        new SqlTableColumn("TranStatus",SqlDbType.SmallInt){ColType= SqlColType.修改},
                                        new SqlTableColumn("IsEnd", SqlDbType.Bit){ColType= SqlColType.修改},
                                        new SqlTableColumn("EndTime", SqlDbType.SmallDateTime){ColType= SqlColType.修改,IsNull=true},
                                        new SqlTableColumn("FailTime",SqlDbType.SmallDateTime){ColType = SqlColType.修改,IsNull=true}
                                };
                                DateTime now = DateTime.Now;
                                list.ForEach(a =>
                                {
                                        if (a.TranStatus == TransactionStatus.已提交)
                                        {
                                                update.AddRow(a.Id, a.TranStatus, true, now, null);
                                        }
                                        else
                                        {
                                                update.AddRow(a.Id, a.TranStatus, false, null, now);
                                        }
                                });
                                return update.Update();
                        }
                }

                public bool GetRetryTran(out Guid[] ids)
                {
                        return this.Get("Id", out ids, new ISqlWhere[] {
                               new SqlWhere("TranStatus",SqlDbType.SmallInt){Value=TransactionStatus.回滚失败},
                               new SqlWhere("IsLock", SqlDbType.Bit){Value=0},
                               new SqlWhere("IsEnd", SqlDbType.Bit){ Value=0}
                        });
                }
                public bool TranRollbackLock(Guid mainTranId, out Guid[] ids)
                {
                        return this.Update(new ISqlSetColumn[] {
                                new SqlSetColumn("TranStatus", SqlDbType.SmallInt){Value=TransactionStatus.待回滚},
                                new SqlSetColumn("FailTime", SqlDbType.SmallDateTime){Value=DateTime.Now},
                                new SqlSetColumn("IsLock", SqlDbType.Bit){Value=1}
                        }, "Id", out ids, new ISqlWhere[] {
                                 new SqlWhere("MainTranId", SqlDbType.UniqueIdentifier){Value=mainTranId},
                                  new SqlWhere("IsLock", SqlDbType.Bit){Value=0}
                        });
                }
                internal bool SetTranExtend(Guid tranId, string extend)
                {
                        return this.Update(new ISqlSetColumn[] {
                                new SqlSetColumn("Extend", SqlDbType.NVarChar){Value=extend}
                        }, new ISqlWhere[] {
                                new SqlWhere("Id", SqlDbType.UniqueIdentifier){Value=tranId},
                                new SqlWhere("TranStatus", SqlDbType.SmallInt){Value=TransactionStatus.执行中}
                        });
                }

                internal bool GetTransaction(Guid id, out TransactionDatum datum)
                {
                        return this.GetRow("Id", id, out datum);
                }
                internal bool GetTranState(Guid id, out RegTranState datum)
                {
                        return this.GetRow("Id", id, out datum);
                }
                public bool AddTran(TransactionList add)
                {
                        return this.Insert(add);
                }
                public bool EndTran(Guid tranId)
                {
                        return this.Update(new ISqlSetColumn[] {
                                new SqlSetColumn("IsEnd", SqlDbType.Bit){Value=1},
                                new SqlSetColumn("EndTime", SqlDbType.SmallDateTime){Value=DateTime.Now}
                        }, new ISqlWhere[] {
                                new SqlWhere("MainTranId", SqlDbType.UniqueIdentifier){Value=tranId},
                                new SqlWhere("TranStatus", SqlDbType.SmallInt){Value=TransactionStatus.已提交}
                        });
                }
                private bool _EndTran(Guid tranId)
                {
                        return this.Update(new ISqlSetColumn[] {
                                new SqlSetColumn("TranStatus", SqlDbType.SmallInt){Value=TransactionStatus.已提交},
                                new SqlSetColumn("IsEnd", SqlDbType.Bit){Value=1},
                                new SqlSetColumn("EndTime", SqlDbType.SmallDateTime){Value=DateTime.Now}
                        }, new ISqlWhere[] {
                                new SqlWhere("MainTranId", SqlDbType.UniqueIdentifier){Value=tranId},
                                new SqlWhere("TranStatus", SqlDbType.SmallInt){Value=TransactionStatus.执行中}
                        });
                }

                public bool SubmitTran(Guid tranId, bool isEnd)
                {
                        if (isEnd)
                        {
                                return this._EndTran(tranId);
                        }
                        return this.SubmitTran(tranId);
                }
               
                public bool SubmitTran(Guid tranId)
                {
                        return this.Update(new ISqlSetColumn[] {
                                           new SqlSetColumn("TranStatus", SqlDbType.SmallInt){Value=TransactionStatus.已提交}
                               }, new ISqlWhere[] {
                                       new SqlWhere("ParentId", SqlDbType.UniqueIdentifier){Value=tranId},
                                       new SqlWhere("TranStatus",SqlDbType.SmallInt){Value=TransactionStatus.执行中}
                               });
                }
                public bool SubmitSubTran(Guid id)
                {
                        if (!this.Update(new ISqlSetColumn[] {
                                  new SqlSetColumn("TranStatus", SqlDbType.SmallInt){Value=TransactionStatus.已提交}
                         }, new ISqlWhere[] {
                                 new SqlWhere("Id", SqlDbType.UniqueIdentifier){Value=id},
                                 new SqlWhere("TranStatus", SqlDbType.SmallInt){Value=TransactionStatus.执行中}
                         }))
                        {
                                return false;
                        }
                        return this.SubmitTran(id);
                }

                public bool RollbackTran(Guid tranId, TransactionStatus status)
                {
                        return this.Update(new ISqlSetColumn[] {
                                new SqlSetColumn("TranStatus", SqlDbType.SmallInt){Value=TransactionStatus.待回滚},
                                new SqlSetColumn("FailTime", SqlDbType.SmallDateTime){Value=DateTime.Now}
                        }, new ISqlWhere[] {
                                 new SqlWhere("ParentId", SqlDbType.UniqueIdentifier){Value=tranId},
                                 new SqlWhere("TranStatus", SqlDbType.SmallInt){Value=status}
                        });
                }
                internal bool RollbackSuccess(Guid tranId)
                {
                        return this.Update(new ISqlSetColumn[] {
                                new SqlSetColumn("TranStatus", SqlDbType.SmallInt){Value=TransactionStatus.已回滚},
                                new SqlSetColumn("IsEnd", SqlDbType.Bit){Value=1},
                                new SqlSetColumn("EndTime", SqlDbType.SmallDateTime){Value=DateTime.Now}
                        }, new ISqlWhere[] {
                                new SqlWhere("Id", SqlDbType.UniqueIdentifier){Value=tranId},
                                new SqlWhere("IsEnd", SqlDbType.SmallInt){Value=0}
                        });
                }
                internal bool SetRollbackState(TransactionDatum tran)
                {
                        return this.Update(new ISqlSetColumn[] {
                               new SqlSetColumn("TranStatus", SqlDbType.SmallInt){Value=tran.TranStatus},
                                new SqlSetColumn("RetryNum", SqlDbType.SmallInt){Value=tran.RetryNum},
                                new SqlSetColumn("ErrorCode", SqlDbType.BigInt){Value=tran.ErrorCode},
                                new SqlSetColumn("IsLock", SqlDbType.Bit){Value=0},
                                new SqlSetColumn("IsEnd", SqlDbType.Bit){Value=tran.IsEnd},
                                new SqlSetColumn("EndTime", SqlDbType.SmallDateTime){Value=tran.EndTime,IsNull=true}
                        }, new ISqlWhere[] {
                                new SqlWhere("Id", SqlDbType.UniqueIdentifier){Value=tran.Id},
                                new SqlWhere("IsEnd", SqlDbType.Bit){Value=0}
                        });
                }
        }
}
