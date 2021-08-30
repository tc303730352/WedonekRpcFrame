using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

using RpcClient.Helper;
using RpcClient.Interface;
using RpcClient.Model;
using RpcClient.Subscribe;

using RpcModel;

using RpcHelper;

namespace RpcClient.Collect
{
        [Attr.ClassLifetimeAttr(Attr.ClassLifetimeType.单例)]
        internal class RpcSubscribeCollect : IRpcSubscribeCollect, IBroadcast
        {
                private static readonly ConcurrentDictionary<string, ISubscribeEvent> _SubEvent = new ConcurrentDictionary<string, ISubscribeEvent>();
                private static IQueueCollect _Queue;
                private static string[] _CacheList = null;

                public static void Init()
                {
                        _Queue = QueueCollect.CreateSubQueue(_QueueMsg);
                        if (!_CacheList.IsNull())
                        {
                                Task.Run(() =>
                                {
                                        _CacheList.ForEach(a =>
                                        {
                                                _Queue.BindRoute(a);
                                        });
                                        _CacheList = null;
                                });
                        }
                        _Queue.Subscribe();
                }

                private static bool _QueueMsg(QueueRemoteMsg obj, string routeKey, string exchange)
                {
                        if (obj.Msg.Source.SourceServerId == RpcClient.CurrentSource.SourceServerId && obj.IsExclude)
                        {
                                return true;
                        }
                        else if (!_SubEvent.TryGetValue(obj.Type, out ISubscribeEvent sub))
                        {
                                return false;
                        }
                        else if (obj.Msg.Tran != null)
                        {
                                return _GetTranMsg(sub, obj);
                        }
                        else
                        {
                                return _GetReply(sub, new RemoteMsg(obj));
                        }
                }
                private static bool _GetTranMsg(ISubscribeEvent sub, QueueRemoteMsg data)
                {
                        using (IRpcTransaction tran = new Tran.RpcQueueTran(data))
                        {
                                if (_GetReply(sub, new RemoteMsg(data)))
                                {
                                        tran.Complate();
                                        return true;
                                }
                                return false;
                        }
                }
                private static bool _GetReply(ISubscribeEvent sub, RemoteMsg msg)
                {
                        if (msg.TcpMsg.IsSync)
                        {
                                bool isOk = true;
                                using (RemoteLock rLock = SyncLockCollect.ApplyLock(msg.TcpMsg.LockId, msg.TcpMsg.LockType))
                                {
                                        if (rLock.GetLock())
                                        {
                                                isOk = sub.Exec(msg);
                                                rLock.Exit(null);
                                        }
                                }
                                return isOk;
                        }
                        else
                        {
                                return sub.Exec(msg);
                        }
                }
                internal static void BindRoute(string name)
                {
                        if (_Queue != null)
                        {
                                _Queue.BindRoute(name);
                        }
                        else
                        {
                                _CacheList = _CacheList.Add(name);
                        }
                }
                public bool CheckIsExists(string name)
                {
                        return _SubEvent.ContainsKey(name);
                }


                public bool Add(string name, Action action)
                {
                        return Add(new AcionDelegate(name, action));
                }
                public bool Add<T>(string name, Action<T> action)
                {
                        return Add(new AcionDelegate(name, action));
                }
                public bool Add<T>(string name, Action<T, MsgSource> action)
                {
                        return Add(new AcionDelegate(name, action));
                }
                public bool Add(string name, Action<MsgSource> action)
                {
                        return Add(new AcionDelegate(name, action));
                }
                public static bool Add(ISubscribeEvent add)
                {
                        if (_SubEvent.ContainsKey(add.EventName))
                        {
                                return false;
                        }
                        else if (add.Init())
                        {
                                return _SubEvent.TryAdd(add.EventName, add);
                        }
                        return false;
                }

                public void BroadcastMsg(IRemoteBroadcast config, DynamicModel body)
                {
                        TcpRemoteMsg msg = RpcClientHelper.GetQueueMsg(config, body);
                        _Queue.Public(config, msg);
                }

                public void BroadcastMsg<T>(IRemoteBroadcast config, T model, long[] serverId)
                {
                        TcpRemoteMsg msg = RpcClientHelper.GetQueueMsg(config, model);
                        _Queue.Public(config, msg, serverId);
                }

                public void BroadcastMsg<T>(IRemoteBroadcast config, T model)
                {
                        TcpRemoteMsg msg = RpcClientHelper.GetQueueMsg(config, model);
                        _Queue.Public(config, msg);
                }

                public void BroadcastMsg<T>(IRemoteBroadcast config, T model, string[] typeVal)
                {
                        TcpRemoteMsg msg = RpcClientHelper.GetQueueMsg(config, model);
                        _Queue.Public(config, msg, typeVal);
                }

                public void Dispose()
                {
                        if (_Queue != null)
                        {
                                _Queue.Dispose();
                        }
                }

                public void BroadcastMsg<T>(IRemoteBroadcast config, T model, long rpcMerId, string[] typeVal)
                {
                        TcpRemoteMsg msg = RpcClientHelper.GetQueueMsg(config, model);
                        _Queue.Public(config, msg, rpcMerId, typeVal);
                }
        }
}
