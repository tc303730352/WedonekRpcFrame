using RpcClient.Interface;
using RpcClient.Model;
using RpcClient.RpcSysEvent;

using RpcModel;

using RpcHelper;

namespace RpcClient.Collect
{
        public delegate IBasicRes ASyncMsgEvent(IMsg msg);
        internal class RpcMsgCollect : IBroadcast
        {
                /// <summary>
                /// 同步的消息事件
                /// </summary>
                private static ASyncMsgEvent _ASyncMsgEvent = null;
                private static readonly IService _Service = RpcService.Service;


                /// <summary>
                /// 初始化消息系统
                /// </summary>
                /// <param name="msg"></param>
                /// <param name="error"></param>
                internal static void SetMsgEvent(ASyncMsgEvent msg)
                {
                        _ASyncMsgEvent = msg;

                }

                private static TcpRemoteReply _GetReply(RemoteMsg msg)
                {
                        TcpRemoteReply result = null;
                        if (msg.TcpMsg.IsSync)
                        {
                                using (RemoteLock rLock = SyncLockCollect.ApplyLock(msg.TcpMsg.LockId, msg.TcpMsg.LockType))
                                {
                                        if (rLock.GetLock())
                                        {
                                                IBasicRes res = _ASyncMsgEvent.Invoke(msg);
                                                if (!msg.TcpMsg.IsReply)
                                                {
                                                        rLock.Exit(null);
                                                }
                                                else
                                                {
                                                        result = new TcpRemoteReply(res);
                                                        if (res.IsError)
                                                        {
                                                                rLock.Exit(result.MsgBody, true);
                                                        }
                                                        else
                                                        {
                                                                rLock.Exit(result.MsgBody);
                                                        }
                                                }
                                        }
                                        else if (!rLock.IsError && msg.TcpMsg.IsReply)
                                        {
                                                result = new TcpRemoteReply
                                                {
                                                        MsgBody = rLock.Extend
                                                };
                                        }
                                        else if (msg.TcpMsg.IsReply)
                                        {
                                                result = new TcpRemoteReply(new BasicRes(rLock.Error));
                                        }
                                }
                        }
                        else
                        {
                                IBasicRes res = _ASyncMsgEvent.Invoke(msg);
                                if (msg.TcpMsg.IsReply)
                                {
                                        result = new TcpRemoteReply(res);
                                }

                        }
                        return result;
                }

                /// <summary>
                /// 消息事件
                /// </summary>
                /// <param name="msg"></param>
                internal static TcpRemoteReply MsgEvent(RemoteMsg msg)
                {
                        if (RemoteSysEvent.MsgEvent(msg, out TcpRemoteReply res))
                        {
                                return res;
                        }
                        else if (!_Service.ReceiveMsgEvent(msg, out string error))
                        {
                                return new TcpRemoteReply(new BasicRes(error));
                        }
                        else if (msg.TcpMsg.Tran != null)
                        {
                                if (!RpcTranCollect.AddTranLog(msg, out error))
                                {
                                        return new TcpRemoteReply(new BasicRes(error));
                                }
                                return _GetReply(msg);
                        }
                        else
                        {
                                return _GetReply(msg);
                        }
                }
                public void BroadcastMsg<T>(IRemoteBroadcast config, T model, long rpcMerId, string[] typeVal)
                {
                        BroadcastMsg msg = new BroadcastMsg(config)
                        {
                                MsgBody = Tools.Json(model),
                                TypeVal = typeVal
                        };
                        msg.RpcMerId = rpcMerId;
                        _BroadcastMsg(msg);
                }
                public void BroadcastMsg<T>(IRemoteBroadcast config, T model, string[] typeVal)
                {
                        BroadcastMsg msg = new BroadcastMsg(config)
                        {
                                MsgBody = Tools.Json(model),
                                TypeVal = typeVal
                        };
                        _BroadcastMsg(msg);
                }
                public void BroadcastMsg(IRemoteBroadcast config, DynamicModel body)
                {
                        BroadcastMsg msg = new BroadcastMsg(config)
                        {
                                MsgBody = body.Json()
                        };
                        _BroadcastMsg(msg);
                }

                public void BroadcastMsg<T>(IRemoteBroadcast config, T model, long[] serverId)
                {
                        BroadcastMsg msg = new BroadcastMsg(config)
                        {
                                MsgBody = Tools.Json(model),
                                ServerId = serverId
                        };
                        _BroadcastMsg(msg);
                }
                public void BroadcastMsg<T>(IRemoteBroadcast config, T model)
                {
                        BroadcastMsg msg = new BroadcastMsg(config)
                        {
                                MsgBody = Tools.Json(model)
                        };
                        _BroadcastMsg(msg);
                }

                private static void _BroadcastMsg(BroadcastMsg msg)
                {
                        if (!RemoteCollect.Send(msg, out string error))
                        {
                                throw new ErrorException(error);
                        }
                }
        }
}
