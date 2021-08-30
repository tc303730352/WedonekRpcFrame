using System;

using RpcClient.Config;
using RpcClient.Helper;
using RpcClient.Interface;
using RpcClient.Model;
using RpcClient.Queue.Model;

using RpcModel;

using RpcHelper;

namespace RpcClient.Collect
{
        [Attr.IgnoreIoc]
        internal class RpcQueueCollect : IBroadcast
        {
                /// <summary>
                /// 同步的消息事件
                /// </summary>
                private static Func<IMsg, IBasicRes> _MsgEvent = null;
                /// <summary>
                /// 数据队列
                /// </summary>
                private static IQueueCollect _Queue = null;



                public static void SetMsgEvent(Func<IMsg, IBasicRes> msgEvent)
                {
                        _MsgEvent = msgEvent;
                }

                public static void InitQueue()
                {
                        QueueConfig config = WebConfig.GetQueueConfig();
                        QueueCollect.InitQueue(config);
                        _Queue = QueueCollect.CreateMsgQueue(_QueueMsg);
                        _Queue.Subscribe();
                }
                /// <summary>
                /// 队列消息事件
                /// </summary>
                /// <param name="data"></param>
                private static bool _QueueMsg(QueueRemoteMsg data, string routeKey, string exchange)
                {
                        if (data.Msg.Source.SourceServerId == RpcClient.CurrentSource.SourceServerId && data.IsExclude)
                        {
                                return true;
                        }
                        IBasicRes res = data.Msg.Tran != null ? _GetTranMsg(data) : _GetReply(new RemoteMsg(data));
                        return !res.IsError;

                }
                private static IBasicRes _GetTranMsg(QueueRemoteMsg data)
                {
                        using (IRpcTransaction tran = new Tran.RpcQueueTran(data))
                        {
                                IBasicRes res = _GetReply(new RemoteMsg(data));
                                if (!res.IsError)
                                {
                                        tran.Complate();
                                }
                                return res;
                        }
                }
                private static IBasicRes _GetReply(RemoteMsg msg)
                {
                        IBasicRes res = null;
                        if (msg.TcpMsg.IsSync)
                        {
                                using (RemoteLock rLock = SyncLockCollect.ApplyLock(msg.TcpMsg.LockId, msg.TcpMsg.LockType))
                                {
                                        if (rLock.GetLock())
                                        {
                                                res = _MsgEvent.Invoke(msg);
                                                rLock.Exit(null);
                                        }
                                }
                                return res;
                        }
                        else
                        {
                                return _MsgEvent.Invoke(msg);
                        }
                }


                public void BroadcastMsg(IRemoteBroadcast config, DynamicModel body)
                {
                        TcpRemoteMsg msg = RpcClientHelper.GetQueueMsg(config, body);
                        if (!_Queue.Public(config, msg))
                        {
                                throw new ErrorException("rpc.broadcast.error");
                        }
                }

                public void BroadcastMsg<T>(IRemoteBroadcast config, T model, long[] serverId)
                {
                        TcpRemoteMsg msg = RpcClientHelper.GetQueueMsg(config, model);
                        if (!_Queue.Public(config, msg, serverId))
                        {
                                throw new ErrorException("rpc.broadcast.error");
                        }
                }

                public void BroadcastMsg<T>(IRemoteBroadcast config, T model)
                {
                        TcpRemoteMsg msg = RpcClientHelper.GetQueueMsg(config, model);
                        if (!_Queue.Public(config, msg))
                        {
                                throw new ErrorException("rpc.broadcast.error");
                        }
                }

                public void BroadcastMsg<T>(IRemoteBroadcast config, T model, string[] typeVal)
                {
                        TcpRemoteMsg msg = RpcClientHelper.GetQueueMsg(config, model);
                        if (!_Queue.Public(config, msg, typeVal))
                        {
                                throw new ErrorException("rpc.broadcast.error");
                        }
                }

                internal static void Dispose()
                {
                        if (_Queue != null)
                        {
                                _Queue.Dispose();
                        }
                }

                public void BroadcastMsg<T>(IRemoteBroadcast config, T model, long rpcMerId, string[] typeVal)
                {
                        TcpRemoteMsg msg = RpcClientHelper.GetQueueMsg(config, model);
                        if (!_Queue.Public(config, msg, rpcMerId, typeVal))
                        {
                                throw new ErrorException("rpc.broadcast.error");
                        }
                }
        }
}
