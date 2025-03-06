using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using WeDonekRpc.Client.Collect;
using WeDonekRpc.Client.Config;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.Ioc;
using WeDonekRpc.Client.Log;
using WeDonekRpc.Client.Model;
using WeDonekRpc.Client.RpcSysEvent;
using WeDonekRpc.Client.Tran;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.IdGenerator;
using WeDonekRpc.Model;
using WeDonekRpc.Model.Tran;
using WeDonekRpc.Model.Tran.Model;

namespace WeDonekRpc.Client.TranService
{
    [Attr.ClassLifetimeAttr(Attr.ClassLifetimeType.SingleInstance)]
    internal class RpcTranService : IRpcTranService
    {
        private static readonly AsyncLocal<ICurTranState> _SourceTran = new AsyncLocal<ICurTranState>(_SourTranChange);


        internal static ICurTranState SourceTran => _SourceTran.Value;
        private static readonly AsyncLocal<ICurTranState> _CurTran = new AsyncLocal<ICurTranState>(_CurTranChange);

        private static readonly ConcurrentDictionary<string, ITranTemplate> _Tran = new ConcurrentDictionary<string, ITranTemplate>();


        public bool IsExecTran => RpcTranService._SourceTran.Value != null;

        private static void _CurTranChange ( AsyncLocalValueChangedArgs<ICurTranState> e )
        {
            if ( e.CurrentValue != null && e.CurrentValue.IsDispose )
            {
                _CurTran.Value = null;
            }
        }
        private static void _SourTranChange ( AsyncLocalValueChangedArgs<ICurTranState> e )
        {
            if ( e.CurrentValue != null && e.CurrentValue.IsDispose )
            {
                _SourceTran.Value = null;
            }
        }
        static RpcTranService ()
        {
            RemoteSysEvent.AddEvent<TranRollback>("Rpc_TranRollback", _TranRollback);
            RemoteSysEvent.AddEvent<TranCommit>("Rpc_TranCommit", _TranSubmit);
        }
        public static long? TranId => RpcTranService._SourceTran.Value?.TranId;
        public static CurTranState GetTranSource ()
        {
            return RpcTranService._SourceTran.Value?.Source;
        }
        private static TcpRemoteReply _TranSubmit ( TranCommit obj, MsgSource source )
        {
            if ( _Tran.TryGetValue(obj.TranName, out ITranTemplate tran) && tran.TranMode != RpcTranMode.NoReg )
            {
                ICurTran cur = new CurState(obj, source);
                using ( IocScope scope = RpcClient.Ioc.CreateScore("Tran_Commit") )
                {
                    tran.Commit(cur);
                }
            }
            return new TcpRemoteReply(new BasicRes());
        }
        private static TcpRemoteReply _TranRollback ( TranRollback obj, MsgSource source )
        {
            if ( _Tran.TryGetValue(obj.TranName, out ITranTemplate tran) && tran.TranMode != RpcTranMode.NoReg )
            {
                ICurTran cur = new CurState(obj, source);
                using ( IocScope scope = RpcClient.Ioc.CreateScore("Tran_Rollback") )
                {
                    tran.Rollback(cur);
                }
            }
            return new TcpRemoteReply(new BasicRes());
        }

        private static bool _ApplyTran ( ITranTemplate template, string data, out ICurTranState tran, out ICurTranState oldTran )
        {
            ApplyTran obj = new ApplyTran
            {
                TranId = IdentityHelper.CreateId(),
                TranName = template.TranName,
                TranMode = template.TranMode,
                SubmitJson = data,
                TimeOut = WebConfig.RpcConfig.TranOverTime,
            };
            if ( !RemoteCollect.Send(obj, out string error) )
            {
                throw new ErrorException(error);
            }
            tran = new TranStateVal(obj.TranId, template, data);
            if ( _BeginLocalTran(tran, out error) )
            {
                oldTran = RpcTranService._SourceTran.Value;
                RpcTranService._SourceTran.Value = tran;
                RpcTranService._CurTran.Value = tran;
                return false;
            }
            throw new ErrorException(error);
        }


        internal static bool ApplyTran ( string tranName, string data, bool isinherit, out ICurTranState tran, out ICurTranState oldTran )
        {
            if ( isinherit && RpcTranService._SourceTran.Value != null )
            {
                oldTran = null;
                if ( RpcTranService._SourceTran.Value is IRpcVirtuallyTransaction virtually )
                {
                    virtually.InitTran(tranName);
                }
                tran = RpcTranService._CurTran.Value;
                return true;
            }
            else if ( !_Tran.TryGetValue(tranName, out ITranTemplate template) )
            {
                throw new ErrorException("rpc.tran.no.reg");
            }
            else
            {
                return _ApplyTran(template, data, out tran, out oldTran);
            }
        }

        internal static void SubmitTran ( ICurTranState tran, ICurTranState source )
        {
            _SetTran(source);
            _SubmitTran(tran);
            if ( tran.Template.TranMode != RpcTranMode.NoReg )
            {
                tran.Template.Commit(tran);
            }
        }
        private static void _SubmitTran ( ICurTranState tran )
        {
            SubmitTran obj = new SubmitTran
            {
                TranId = tran.TranId
            };
            if ( !RemoteCollect.Send(tran.RpcMerId, tran.RegionId, obj, out string error) )
            {
                throw new ErrorException(error)
                {
                    Args = new Dictionary<string, string>
                    {
                        {"TranId",tran.TranId.ToString() },
                        {"RegionId",tran.RegionId.ToString() },
                        {"RpcMerId",tran.RpcMerId.ToString() }
                    }
                };
            }
        }
        private static void _SetTran ( ICurTranState tran )
        {
            RpcTranService._SourceTran.Value = tran;
            RpcTranService._CurTran.Value = tran;
        }

        private static void _Rollback ( ICurTranState tran )
        {
            RollbackTran obj = new RollbackTran
            {
                TranId = tran.TranId
            };
            if ( !RemoteCollect.Send(tran.RpcMerId, tran.RegionId, obj, out string error) )
            {
                throw new ErrorException(error)
                {
                    Args = new Dictionary<string, string>
                    {
                        {"TranId",tran.TranId.ToString() },
                        {"RegionId",tran.RegionId.ToString() },
                        {"RpcMerId",tran.RpcMerId.ToString() }
                    }
                };
            }
        }


        internal static void RollbackTran ( ICurTranState tran, ICurTranState source )
        {
            _SetTran(source);
            _Rollback(tran);
            if ( tran.Template.TranMode != RpcTranMode.NoReg )
            {
                tran.Template.Rollback(tran);
            }
        }

        internal static ICurTranState AddTranLog ( CurTranState state, ITranSource source, string tranName )
        {
            if ( !_Tran.TryGetValue(tranName, out ITranTemplate template) )
            {
                throw new ErrorException("rpc.tran.no.reg");
            }
            AddTranLog log = new AddTranLog
            {
                TranId = state.TranId,
                TranLog = new TranLogDatum
                {
                    TranMode = template.TranMode,
                    Dictate = tranName,
                    RpcMerId = RpcStateCollect.RpcMerId,
                    ServerId = RpcStateCollect.ServerId,
                    SystemType = RpcStateCollect.LocalConfig.SystemType,
                    SubmitJson = source.Body
                }
            };
            if ( !RemoteCollect.Send(state.RpcMerId, state.RegionId, log, out long id, out string error) )
            {
                throw new ErrorException(error);
            }
            ICurTranState cur = new TranStateVal(id, template, state, source);
            if ( _BeginLocalTran(cur, out error) )
            {
                RpcTranService._CurTran.Value = cur;
                return cur;
            }
            throw new ErrorException(error);
        }

        internal static bool AddTranLog ( RemoteMsg msg, out ICurTranState cur, out string error )
        {
            if ( !_Tran.TryGetValue(msg.MsgKey, out ITranTemplate template) )
            {
                RpcTranService._SourceTran.Value = new VirtuallyTransaction(msg.TcpMsg.Tran, msg.MsgBody);
                error = null;
                cur = RpcTranService._SourceTran.Value;
                return true;
            }
            CurTranState tran = msg.TcpMsg.Tran;
            AddTranLog log = new AddTranLog
            {
                TranId = tran.TranId,
                TranLog = new TranLogDatum
                {
                    TranMode = template.TranMode,
                    Dictate = msg.MsgKey,
                    RpcMerId = RpcStateCollect.RpcMerId,
                    ServerId = RpcStateCollect.ServerId,
                    SystemType = RpcStateCollect.LocalConfig.SystemType,
                    SubmitJson = msg.MsgBody
                }
            };
            if ( RemoteCollect.Send(tran.RpcMerId, tran.RegionId, log, out long id, out error) )
            {
                cur = new TranStateVal(id, template, tran, msg.MsgBody);
                if ( _BeginLocalTran(cur, out error) )
                {
                    RpcTranService._SourceTran.Value = new TranStateVal(tran);
                    RpcTranService._CurTran.Value = cur;
                    return true;
                }
            }
            cur = null;
            return false;
        }
        private static bool _BeginLocalTran ( ICurTranState cur, out string error )
        {
            try
            {
                cur.BeginTran();
                error = null;
                return true;
            }
            catch ( Exception e )
            {
                RpcLogSystem.AddErrorLog("启动事务错误", e);
                error = "rpc.tran.begin.fail";
                return false;
            }
        }
        public static void SetTranExtend ( ICurTranState tran, string extend )
        {
            SetTranExtend set = new SetTranExtend
            {
                TranId = tran.TranId,
                Extend = extend
            };
            if ( !RemoteCollect.Send(tran.RpcMerId, tran.RegionId, set, out string error) )
            {
                throw new ErrorException(error);
            }
            tran.Body.Extend = extend;
        }
        public void SetTranExtend<T> ( T extend ) where T : class
        {
            if ( RpcTranService._CurTran.Value != null )
            {
                SetTranExtend(RpcTranService._CurTran.Value, extend.ToJson());
            }
        }


        public void SetTranExtend ( string extend )
        {
            if ( RpcTranService._CurTran.Value != null )
            {
                SetTranExtend(RpcTranService._CurTran.Value, extend);
            }
        }

        public void SetTranExtend ( Dictionary<string, object> extend )
        {
            if ( RpcTranService._CurTran.Value != null )
            {
                SetTranExtend(RpcTranService._CurTran.Value, extend.ToJson());
            }
        }
        public static void RegTran ( string name, ITranTemplate template )
        {
            if ( _Tran.ContainsKey(name) )
            {
                throw new WebException("rpc.tran.repeat");
            }
            else if ( !_Tran.TryAdd(name, template) )
            {
                throw new WebException("rpc.tran.reg.fail");
            }
        }


        public TransactionStatus GetTranState ()
        {
            ICurTranState tran = RpcTranService._CurTran.Value;
            if ( tran == null )
            {
                throw new ErrorException("rpc.tran.no.find");
            }
            return this._GetTranState(tran.TranId, tran.RpcMerId, tran.RegionId);
        }
        public bool GetTranState ( CurTranState state, out TransactionStatus status, out string error )
        {
            GetTranState obj = new GetTranState
            {
                TranId = state.TranId
            };
            return RemoteCollect.Send(state.RpcMerId, state.RegionId, obj, out status, out error);
        }
        private TransactionStatus _GetTranState ( long tranId, long rpcMerId, int regionId )
        {
            GetTranState obj = new GetTranState
            {
                TranId = tranId
            };
            if ( !RemoteCollect.Send(rpcMerId, regionId, obj, out TransactionStatus status, out string error) )
            {
                throw new ErrorException(error);
            }
            return status;
        }

        public static void Dispose ()
        {
            RpcTranService._CurTran.Value = null;
            RpcTranService._SourceTran.Value = null;
        }
        public static void ReceiveEnd ()
        {
            if ( RpcTranService._CurTran.Value != null )
            {
                _SetTran(null);
            }
        }


    }
}
