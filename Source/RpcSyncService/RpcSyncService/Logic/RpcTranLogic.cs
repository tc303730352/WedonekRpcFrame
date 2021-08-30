using System;
using System.Text;
using System.Threading.Tasks;

using RpcModel;
using RpcModel.Tran.Model;

using RpcSyncService.Collect;
using RpcSyncService.Model;
using RpcSyncService.Tran;

using RpcHelper;

namespace RpcSyncService.Logic
{
        internal class RpcTranLogic
        {
                public static void TranLockOverTime()
                {
                        Guid[] tranId = TransactionCollect.GetLockOverTimeTran();
                        if (!tranId.IsNull())
                        {
                                SingleTranRollbackQueue.AddQueue(tranId);
                        }
                }

                internal static TranResult GetTranResult(Guid tranId)
                {
                        RegTranState state = TransactionCollect.GetTranState(tranId);
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
                                TranStatus = RpcModel.TransactionStatus.执行中,
                                BeginTime = state.AddTime,
                                Logs = TransactionCollect.GetTranLogs(tranId)
                        };
                }

                /// <summary>
                /// 检查挂起的事务状态
                /// </summary>
                public static void CheckHangUpTran()
                {
                        Guid[] tranId = TransactionCollect.GetHangUpTran();
                        if (!tranId.IsNull())
                        {
                                Task.Run(() =>
                                {
                                        _SyncState(tranId);
                                });
                        }
                }
                /// <summary>
                /// 重启回滚失败的事务
                /// </summary>
                public static void RestartRetryTran()
                {
                        Guid[] tranId = TransactionCollect.GetRetryTran();
                        if (!tranId.IsNull())
                        {
                                SingleTranRollbackQueue.AddQueue(tranId);
                        }
                }

                internal static RpcTranState GetTranState(Guid tranId)
                {
                        RegTranState state = TransactionCollect.GetTranState(tranId);
                        return new RpcTranState
                        {
                                TranStatus = state.TranStatus,
                                IsEnd = state.IsEnd,
                                BeginTime = state.AddTime,
                                OverTime = state.OverTime
                        };
                }

                /// <summary>
                /// 检查超时的事务
                /// </summary>
                public static void CheckOverTimeTran()
                {
                        Guid[] tranId = TransactionCollect.GetOverTimeTran();
                        if (!tranId.IsNull())
                        {
                                SingleTranRollbackQueue.AddQueue(tranId);
                        }
                }
                private static void _SyncState(Guid[] tranId)
                {
                        Model.TranState[] states = TransactionCollect.GetTranState(tranId);
                        if (states == null)
                        {
                                return;
                        }
                        SetTranState[] sets = tranId.ConvertAll(a =>
                          {
                                  Model.TranState[] st = states.FindAll(b => b.ParentId == a);
                                  TransactionStatus status = TransactionStatus.已提交;
                                  if (st.IsExists(b => b.TranStatus != TransactionStatus.已提交))
                                  {
                                          status = TransactionStatus.待回滚;
                                  }
                                  return new SetTranState { Id = a, TranStatus = status };
                          });
                        if (!TransactionCollect.SetTranState(sets))
                        {
                                new WarnLog("rpc.tran.state.set.error")
                                {
                                        LogTitle = "事务状态设置出错",
                                        LogContent = sets.ToJson()
                                }.Save();
                                return;
                        }
                        Guid[] ids = sets.Convert(a => a.TranStatus == TransactionStatus.待回滚, a => a.Id);
                        if (ids.Length > 0)
                        {
                                TranRollbackQueue.AddQueue(ids);
                        }
                }
        }
}
