using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;

using RpcClient.Config;
using RpcClient.Interface;
using RpcClient.Model;
using RpcClient.RpcSysEvent;
using RpcClient.Tran;

using RpcModel;
using RpcModel.Tran;
using RpcModel.Tran.Model;

using RpcHelper;

namespace RpcClient.Collect
{
        [Attr.ClassLifetimeAttr(Attr.ClassLifetimeType.单例)]
        internal class RpcTranCollect : IRpcTranCollect
        {
                [ThreadStatic]
                internal static CurTranState Tran = null;
                [ThreadStatic]
                private static CurTranState _CurTran;

                private static readonly Dictionary<string, ITranTemplate> _Tran = new Dictionary<string, ITranTemplate>();

                public bool IsExecTran => Tran != null;

                static RpcTranCollect()
                {
                        RemoteSysEvent.AddEvent<TranRollback>("Rpc_TranRollback", _TranRollback);
                }

                private static TcpRemoteReply _TranRollback(TranRollback obj)
                {
                        if (_Tran.TryGetValue(obj.TranName, out ITranTemplate tran))
                        {
                                tran.Rollback(obj.SubmitJson, obj.Extend);
                        }
                        return new TcpRemoteReply(new BasicRes());
                }
                public void RegTran<T>(Action<T, string> rollback) where T : class
                {
                        Type type = typeof(T);
                        this.RegTran(type.Name, rollback);
                }
                public void RegTran<T>(string name, Action<T, string> rollback) where T : class
                {
                        _Tran.Add(name, new TranTemplate<T>(name, rollback));
                }

                internal static void EndTran(CurTranState tran)
                {
                        EndTran obj = new EndTran
                        {
                                TranId = tran.TranId
                        };
                        if (!RemoteCollect.Send(tran.RpcMerId, tran.RegionId, obj, out string error))
                        {
                                throw new ErrorException(error);
                        }
                }

                public static CurTranState ApplyQueueTran(QueueRemoteMsg msg)
                {
                        CurTranState tran = msg.Msg.Tran;
                        if (!tran.MainTranId.HasValue)
                        {
                                tran.MainTranId = tran.TranId;
                        }
                        return _ApplyQueueTran(tran, msg);
                }
                private static CurTranState _ApplyQueueTran(CurTranState tran, QueueRemoteMsg msg)
                {
                        ApplyTran obj = new ApplyTran
                        {
                                TranId = Tools.NewGuid(),
                                TranName = msg.Type,
                                Level = tran.Level,
                                MainTranId = tran.MainTranId,
                                SubmitJson = msg.Msg.MsgBody,
                                IsReg = _Tran.ContainsKey(msg.Type)
                        };
                        if (!RemoteCollect.Send(tran.RpcMerId, tran.RegionId, obj, out string error))
                        {
                                throw new ErrorException(error);
                        }
                        tran.TranId = obj.TranId;
                        RpcTranCollect._CurTran = tran;
                        RpcTranCollect.Tran = tran;
                        return tran;
                }


                internal static bool ApplyTran(bool isinherit, TranLevel level, out CurTranState tran, out CurTranState oldTran)
                {
                        if (isinherit && RpcTranCollect.Tran != null)
                        {
                                oldTran = null;
                                tran = RpcTranCollect.Tran;
                                return true;
                        }
                        return _ApplyTran("DefTran", level, null, false, out tran, out oldTran);
                }

                internal static TranStatus WaitEnd(CurTranState tran, Action<IRpcTran> checkTran, int waitTime)
                {
                        int time = HeartbeatTimeHelper.HeartbeatTime + waitTime;
                        Thread.SpinWait(20);
                        do
                        {
                                TranResult result = _GetTranResult(tran);
                                if (result.TranStatus == TransactionStatus.已提交 || result.TranStatus == TransactionStatus.执行中)
                                {
                                        IRpcTran tr = new RpcTran(tran, result);
                                        checkTran(tr);
                                        if (tr.TranIsEnd)
                                        {
                                                return tr.TranStatus;
                                        }
                                        if (time <= HeartbeatTimeHelper.HeartbeatTime)
                                        {
                                                return TranStatus.超时;
                                        }
                                        Thread.Sleep(100);
                                }
                                else
                                {
                                        return TranStatus.回滚;
                                }
                        } while (true);
                }

                private static TranResult _GetTranResult(CurTranState tran)
                {
                        if (!RemoteCollect.Send(tran.RpcMerId, tran.RegionId, new GetTranResult
                        {
                                TranId = tran.TranId
                        }, out TranResult result, out string error))
                        {
                                throw new ErrorException(error);
                        }
                        return result;
                }

                private static bool _ApplyTran(string tranName, TranLevel level, string data, bool isReg, out CurTranState tran, out CurTranState oldTran)
                {
                        ApplyTran obj = new ApplyTran
                        {
                                TranId = Tools.NewGuid(),
                                TranName = tranName,
                                Level = level,
                                IsReg = isReg,
                                SubmitJson = data,
                                OverTime = WebConfig.RpcConfig.TranOverTime,
                        };
                        if (!RemoteCollect.Send(obj, out string error))
                        {
                                throw new ErrorException(error);
                        }
                        tran = new CurTranState
                        {
                                TranId = obj.TranId,
                                Level = obj.Level,
                                RegionId = RpcStateCollect.ServerConfig.RegionId,
                                RpcMerId = RpcStateCollect.RpcMerId
                        };
                        oldTran = RpcTranCollect.Tran;
                        RpcTranCollect.Tran = tran;
                        RpcTranCollect._CurTran = tran;
                        return false;
                }
                internal static bool ApplyTran(string tranName, TranLevel level, string data, bool isinherit, out CurTranState tran, out CurTranState oldTran)
                {
                        if (!_Tran.ContainsKey(tranName))
                        {
                                throw new ErrorException("rpc.tran.no.reg");
                        }
                        if (isinherit && RpcTranCollect.Tran != null)
                        {
                                oldTran = null;
                                tran = RpcTranCollect.Tran;
                                return true;
                        }
                        return _ApplyTran(tranName, level, data, true, out tran, out oldTran);
                }

                internal static void SubmitTran(CurTranState tran, CurTranState source)
                {
                        SubmitTran obj = new SubmitTran
                        {
                                TranId = tran.TranId
                        };
                        if (!RemoteCollect.Send(tran.RpcMerId, tran.RegionId, obj, out string error))
                        {
                                _SetTran(source);
                                throw new ErrorException(error);
                        }
                        _SetTran(source);
                }
                private static void _SetTran(CurTranState tran)
                {
                        RpcTranCollect.Tran = tran;
                        RpcTranCollect._CurTran = tran;
                }
                internal static void RollbackTran(CurTranState tran)
                {
                        if (tran == null)
                        {
                                return;
                        }
                        RollbackTran obj = new RollbackTran
                        {
                                TranId = tran.TranId
                        };
                        if (!RemoteCollect.Send(tran.RpcMerId, tran.RegionId, obj, out string error))
                        {
                                _SetTran(null);
                                throw new ErrorException(error);
                        }
                        _SetTran(null);
                }
                internal static void ForceRollbackTran(CurTranState tran)
                {
                        ForceRollbackTran obj = new ForceRollbackTran
                        {
                                TranId = tran.TranId
                        };
                        if (!RemoteCollect.Send(tran.RpcMerId, tran.RegionId, obj, out string error))
                        {
                                throw new ErrorException(error);
                        }
                }

                internal static void RollbackTran(CurTranState tran, CurTranState source)
                {
                        RollbackTran obj = new RollbackTran
                        {
                                TranId = tran.TranId
                        };
                        if (!RemoteCollect.Send(tran.RpcMerId, tran.RegionId, obj, out string error))
                        {
                                _SetTran(source);
                                throw new ErrorException(error);
                        }
                        _SetTran(source);
                }


                internal static bool AddTranLog(RemoteMsg msg, out string error)
                {
                        if (!_Tran.ContainsKey(msg.MsgKey))
                        {
                                error = null;
                                return true;
                        }
                        CurTranState tran = msg.TcpMsg.Tran;
                        AddTranLog log = new AddTranLog
                        {
                                TranId = tran.TranId,
                                MainTranId = tran.MainTranId,
                                TranLog = new TranLogDatum
                                {
                                        Dictate = msg.MsgKey,
                                        RpcMerId = RpcStateCollect.RpcMerId,
                                        ServerId = RpcStateCollect.ServerId,
                                        SystemType = RpcStateCollect.LocalSource.SystemType,
                                        SubmitJson = msg.MsgBody
                                }
                        };
                        if (RemoteCollect.Send(tran.RpcMerId, tran.RegionId, log, out Guid id, out error))
                        {
                                Tran = tran;
                                _CurTran = new CurTranState
                                {
                                        MainTranId = tran.MainTranId,
                                        RegionId = tran.RegionId,
                                        RpcMerId = tran.RpcMerId,
                                        Level = tran.Level,
                                        TranId = id
                                };
                                return true;
                        }
                        return false;
                }
                public static void SetTranExtend(CurTranState tran, string extend)
                {
                        SetTranExtend set = new SetTranExtend
                        {
                                TranId = tran.TranId,
                                Extend = extend
                        };
                        if (!RemoteCollect.Send(tran.RpcMerId, tran.RegionId, set, out string error))
                        {
                                throw new ErrorException(error);
                        }
                }
                public void SetTranExtend<T>(T extend) where T : class
                {
                        if (_CurTran == null)
                        {
                                return;
                        }
                        SetTranExtend(_CurTran, extend.ToJson());
                }


                public void SetTranExtend(string extend)
                {
                        if (_CurTran == null)
                        {
                                return;
                        }
                        SetTranExtend(_CurTran, extend);
                }

                public void SetTranExtend(Dictionary<string, object> extend)
                {
                        if (_CurTran == null)
                        {
                                return;
                        }
                        SetTranExtend(_CurTran, extend.ToJson());
                }

                public void RegTran<T, Extend>(string name, Action<T, Extend> rollback) where T : class
                {
                        _Tran.Add(name, new BasicTranTemplate<T, Extend>(name, rollback));
                }

                public void RegTran<T, Extend>(Action<T, Extend> rollback) where T : class
                {
                        Type type = typeof(T);
                        this.RegTran(type.Name, rollback);
                }

                public void RegTran(string name, Action<string> rollback)
                {
                        _Tran.Add(name, new NoParamTranTemplate(name, rollback));
                }

                public void RegTran<Extend>(string name, Action<Extend> rollback)
                {
                        _Tran.Add(name, new NoParamTranTemplate<Extend>(name, rollback));
                }

                public RpcTranState GetTranState()
                {
                        if (_CurTran == null)
                        {
                                throw new ErrorException("rpc.tran.no.find");
                        }
                        return this._GetTranState(_CurTran.TranId, _CurTran.RpcMerId, _CurTran.RegionId);
                }
                private RpcTranState _GetTranState(Guid tranId,long rpcMerId,int regionId)
                {
                        GetTranState obj = new GetTranState
                        {
                                TranId = tranId
                        };
                        if (!RemoteCollect.Send(rpcMerId, regionId, obj, out RpcTranState state, out string error))
                        {
                                throw new ErrorException(error);
                        }
                        return state;
                }
                public RpcTranState GetTranState(ICurTran tran)
                {
                        return this._GetTranState(tran.TranId, tran.RpcMerId, tran.RegionId);
                }
        }
}
