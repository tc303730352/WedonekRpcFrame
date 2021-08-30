using System;

using RpcModel;
using RpcModel.Tran;
using RpcModel.Tran.Model;

using RpcSyncService.DAL;
using RpcSyncService.Model;
using RpcSyncService.Tran;

using RpcHelper;
namespace RpcSyncService.Collect
{
        internal class TransactionCollect
        {
                [ThreadStatic]
                private static DAL.TransactionDAL _TranDAL = null;

                private static TransactionDAL TranDAL
                {
                        get
                        {
                                if (_TranDAL == null)
                                {
                                        _TranDAL = new DAL.TransactionDAL();
                                }
                                return _TranDAL;
                        }
                }

                public static Guid[] GetHangUpTran()
                {
                        if (!TranDAL.GetHangUpTran(out Guid[] tranId))
                        {
                                throw new ErrorException("rpc.hangUp.tran.get.error");
                        }
                        return tranId;

                }
                public static Guid[] GetLockOverTimeTran()
                {
                        if (!TranDAL.GetLockOverTimeTran(out Guid[] tranId))
                        {
                                throw new ErrorException("rpc.overtime.tran.get.error");
                        }
                        return tranId;
                }
                public static bool SetTranState(SetTranState[] sets)
                {
                        return TranDAL.SetTranState(sets);
                }

                internal static Guid[] GetRetryTran()
                {
                        if (!TranDAL.GetRetryTran(out Guid[] tran))
                        {
                                throw new ErrorException("rpc.retry.tran.get.error");
                        }
                        return tran;
                }

                internal static Guid[] GetOverTimeTran()
                {
                        if (!TranDAL.GetOverTimeTran(out Guid[] tran))
                        {
                                new LogInfo(LogGrade.ERROR, "Transaction")
                                {
                                        LogTitle = "获取超时事务错误!"
                                }.Save();
                                return null;
                        }
                        return tran;
                }

                internal static void EndTran(Guid tranId)
                {
                        RegTranState tran = GetTranState(tranId);
                        tran.CheckIsExec();
                        if (tran.MainTranId != tranId)
                        {
                                return;
                        }
                        else if (!TranDAL.EndTran(tranId))
                        {
                                throw new ErrorException("rpc.tran.end.error");
                        }
                        else
                        {
                                tran.Refresh();
                        }
                }

                public static TranLog[] GetTranLogs(Guid tranId)
                {
                        return TranDAL.GetTranLogs(tranId);
                }
                public static Model.TranState[] GetTranState(Guid[] tranId)
                {
                        if (TranDAL.GetTranState(tranId, out Model.TranState[] states))
                        {
                                return states;
                        }
                        return null;
                }





                internal static void SetTranExtend(Guid tranId, string extend)
                {
                        if (!TranDAL.SetTranExtend(tranId, extend))
                        {
                                throw new ErrorException("rpc.tran.set.error");
                        }
                }



                public static TransactionDatum GetTransaction(Guid id)
                {
                        string key = string.Concat("Tran_", id.ToString("N"));
                        if (RpcClient.RpcClient.Cache.TryGet(key, out TransactionDatum datum))
                        {
                                return datum;
                        }
                        else if (!TranDAL.GetTransaction(id, out datum) || datum == null)
                        {
                                return null;
                        }
                        else
                        {
                                RpcClient.RpcClient.Cache.Add(key, datum, new TimeSpan(0, 30, 0));
                                return datum;
                        }
                }
                /// <summary>
                /// 回滚主事务返回受影响的事务ID
                /// </summary>
                /// <param name="mainTranId"></param>
                /// <returns></returns>
                public static Guid[] TranRollbackLock(Guid mainTranId)
                {
                        if (!TranDAL.TranRollbackLock(mainTranId, out Guid[] ids))
                        {
                                throw new ErrorException("rpc.tran.rollback.set.error");
                        }
                        return ids;
                }



                public static Guid AddTranLog(Guid tranId, TranLogDatum datum, MsgSource source)
                {
                        RegTranState state = GetTranState(tranId);
                        if (state == null)
                        {
                                throw new ErrorException("rpc.tran.not.find");
                        }
                        state.CheckIsEnd();
                        string key = string.Join("_", "tran", tranId, datum.Dictate, source.RpcMerId, source.SystemTypeId);
                        Guid id = _CheckIsRepeat(key, tranId, datum, source);
                        if (id != Guid.Empty)
                        {
                                return id;
                        }
                        else
                        {
                                id = _AddTranLog(state, datum, source);
                                RpcClient.RpcClient.Cache.Add(key, id, new TimeSpan(0, 10, 0));
                                return id;
                        }
                }


                internal static bool RollbackResult(TransactionDatum tran)
                {
                        if (!_RollbackResult(tran))
                        {
                                return false;
                        }
                        string key = string.Concat("Tran_", tran.Id.ToString("N"));
                        if (tran.TranStatus != TransactionStatus.已回滚)
                        {
                                RpcClient.RpcClient.Cache.Set(key, tran, new TimeSpan(0, 10, 0));
                        }
                        else
                        {
                                RpcClient.RpcClient.Cache.Remove(key);
                        }
                        return true;
                }
                private static bool _RollbackResult(TransactionDatum tran)
                {
                        if (tran.TranStatus == TransactionStatus.已回滚)
                        {
                                return new DAL.TransactionDAL().RollbackSuccess(tran.Id);
                        }
                        else
                        {
                                return new DAL.TransactionDAL().SetRollbackState(tran);
                        }
                }
                /// <summary>
                /// 提交事务
                /// </summary>
                /// <param name="tranId"></param>
                public static void SubmitTran(Guid tranId)
                {
                        RegTranState tran = GetTranState(tranId);
                        tran.CheckState();
                        if (tran.TranStatus == TransactionStatus.已提交)
                        {
                                return;
                        }
                        bool isEnd = tran.Level == TranLevel.RPC消息;
                        if (tran.MainTranId != tranId)
                        {
                                RegTranState main = GetTranState(tran.MainTranId);
                                if (main.TranStatus == TransactionStatus.待回滚)
                                {
                                        _Rollback(tran);
                                        return;
                                }
                                isEnd = false;
                        }
                        if (!TranDAL.SubmitTran(tranId, isEnd))
                        {
                                throw new ErrorException("rpc.tran.submit.error");
                        }
                        tran.Refresh();
                }

                /// <summary>
                /// 提交主事务
                /// </summary>
                /// <param name="tranId"></param>
                public static void _SubmitMainTran(RegTranState tran)
                {
                        bool isEnd = tran.Level == TranLevel.RPC消息;
                        if (!TranDAL.SubmitTran(tran.Id, isEnd))
                        {
                                throw new ErrorException("rpc.tran.submit.error");
                        }
                        tran.Refresh();
                }
                /// <summary>
                /// 回滚事务
                /// </summary>
                /// <param name="tranId"></param>
                public static void ForceRollbackTran(Guid tranId)
                {
                        RegTranState tran = GetTranState(tranId);
                        tran.CheckIsAllowRollback();
                        _Rollback(tran);
                }
                /// <summary>
                /// 回滚事务
                /// </summary>
                /// <param name="tranId"></param>
                public static void RollbackTran(Guid tranId)
                {
                        RegTranState tran = GetTranState(tranId);
                        tran.CheckState();
                        _Rollback(tran);
                }

                private static void _Rollback(RegTranState tran)
                {
                        if (!TranDAL.RollbackTran(tran.Id, tran.TranStatus))
                        {
                                throw new ErrorException("rpc.tran.rollback.error");
                        }
                        else
                        {
                                tran.Refresh();
                                TranRollbackQueue.AddQueue(tran);
                        }
                }
                public static void ApplyTransaction(MsgSource source, ApplyTran apply)
                {
                        string key = string.Concat("TranState_", apply.TranId.ToString("N"));
                        if (RpcClient.RpcClient.Cache.TryGet(key, out RegTranState tran))
                        {
                                tran.CheckState();
                        }
                        tran = _ApplyTransaction(source, apply);
                        RpcClient.RpcClient.Cache.Add(key, tran, new TimeSpan(0, 10, 0));
                }
                public static RegTranState GetTranState(Guid id)
                {
                        string key = string.Concat("TranState_", id.ToString("N"));
                        if (RpcClient.RpcClient.Cache.TryGet(key, out RegTranState tran))
                        {
                                return tran;
                        }
                        else if (!TranDAL.GetTranState(id, out tran))
                        {
                                throw new ErrorException("rpc.tran.get.error");
                        }
                        else if (tran == null)
                        {
                                throw new ErrorException("rpc.tran.not.find");
                        }
                        else
                        {
                                RpcClient.RpcClient.Cache.Add(key, tran, new TimeSpan(0, 30, 0));
                                return tran;
                        }
                }
                private static RegTranState _ApplyTransaction(MsgSource source, ApplyTran apply)
                {
                        DateTime now = DateTime.Now;
                        TransactionList tran = new TransactionList
                        {
                                TranName = apply.TranName,
                                RpcMerId = source.RpcMerId,
                                SubmitJson = apply.SubmitJson,
                                SystemTypeId = source.SystemTypeId,
                                IsMainTran = true,
                                Level = apply.Level,
                                AddTime = now,
                                OverTime = now.AddSeconds(apply.OverTime),
                                ServerId = source.SourceServerId,
                                SystemType = source.SystemType,
                                IsRegTran = apply.IsReg,
                                TranStatus = TransactionStatus.执行中,
                                Id = apply.TranId,
                                ParentId = apply.TranId,
                                RegionId = source.RegionId,
                                MainTranId = apply.MainTranId ?? apply.TranId
                        };
                        if (!new DAL.TransactionDAL().AddTran(tran))
                        {
                                throw new ErrorException("rpc.tran.add.error");
                        }
                        return new RegTranState
                        {
                                Id = tran.Id,
                                AddTime = now,
                                MainTranId = tran.MainTranId,
                                Level = tran.Level,
                                IsEnd = false,
                                OverTime = tran.OverTime
                        };
                }

                private static Guid _CheckIsRepeat(string key, Guid tranId, TranLogDatum datum, MsgSource source)
                {
                        if (RpcClient.RpcClient.Cache.TryGet(key, out Guid id))
                        {
                                return id;
                        }
                        else if (!TranDAL.CheckIsRepeat(tranId, datum.Dictate, source, out id))
                        {
                                throw new ErrorException("rpc.tran.check.error");
                        }
                        else if (id != Guid.Empty)
                        {
                                RpcClient.RpcClient.Cache.Add(key, id, new TimeSpan(0, 10, 0));
                        }
                        return id;
                }

                private static Guid _AddTranLog(RegTranState tran, TranLogDatum datum, MsgSource source)
                {
                        TransactionList log = new TransactionList
                        {
                                TranName = datum.Dictate,
                                IsRegTran = true,
                                MainTranId = tran.MainTranId,
                                RpcMerId = source.RpcMerId,
                                SystemTypeId = source.SystemTypeId,
                                Level = tran.Level,
                                RegionId = source.RegionId,
                                AddTime = DateTime.Now,
                                OverTime = tran.OverTime,
                                ServerId = datum.ServerId,
                                SubmitJson = datum.SubmitJson,
                                SystemType = datum.SystemType,
                                ParentId = tran.Id,
                                TranStatus = tran.TranStatus,
                                Id = Tools.NewGuid()
                        };
                        if (!TranDAL.AddTran(log))
                        {
                                throw new ErrorException("rpc.tran.add.error");
                        }
                        return log.Id;
                }
        }
}
