using System;
using System.Collections.Concurrent;
using WeDonekRpc.Client.Helper;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.Model;
using WeDonekRpc.Client.Subscribe;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;

namespace WeDonekRpc.Client.Collect
{
    [Attr.ClassLifetimeAttr(Attr.ClassLifetimeType.SingleInstance)]
    internal class RpcSubscribeCollect : IRpcSubscribeCollect, IBroadcast
    {
        private static readonly ConcurrentDictionary<string, ISubscribeEvent> _SubEvent = new ConcurrentDictionary<string, ISubscribeEvent>();
        private static IQueueCollect _Queue;
        private static string[] _CacheList = null;

        public static void Init ()
        {
            _Queue = QueueCollect.CreateSubQueue(_QueueMsg);
            if (!_CacheList.IsNull())
            {
                _CacheList.ForEach(a =>
                {
                    _Queue.BindRoute(a);
                });
                _CacheList = null;
            }
            _Queue.Subscribe();
        }

        private static SubscribeErrorType _QueueMsg (QueueRemoteMsg obj, string routeKey, string exchange)
        {
            if (obj.Msg.Source.ServerId == RpcClient.CurrentSource.ServerId && obj.IsExclude)
            {
                return SubscribeErrorType.Exclude;
            }
            else if (!_SubEvent.TryGetValue(obj.Type, out ISubscribeEvent sub))
            {
                return SubscribeErrorType.NotFind;
            }
            else
            {
                return _GetReply(sub, new RemoteMsg(obj.Type, obj.Msg)) ? SubscribeErrorType.Success : SubscribeErrorType.Fail;
            }
        }

        private static bool _GetReply (ISubscribeEvent sub, RemoteMsg msg)
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
        internal static void BindRoute (string name)
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
        public bool CheckIsExists (string name)
        {
            return _SubEvent.ContainsKey(name);
        }


        public bool Add (string name, Action action)
        {
            return Add(new AcionDelegate(name, action));
        }
        public bool Add<T> (string name, Action<T> action)
        {
            return Add(new AcionDelegate(name, action));
        }
        public bool Add<T> (string name, Action<T, MsgSource> action)
        {
            return Add(new AcionDelegate(name, action));
        }
        public bool Add (string name, Action<MsgSource> action)
        {
            return Add(new AcionDelegate(name, action));
        }
        internal static bool Add (ISubscribeEvent add)
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

        public void BroadcastMsg (IRemoteBroadcast config, DynamicModel body)
        {
            TcpRemoteMsg msg = RpcClientHelper.GetQueueMsg(config, body);
            this._BroadcastMsg(config, msg);
        }

        public void BroadcastMsg<T> (IRemoteBroadcast config, T model, long[] serverId)
        {
            TcpRemoteMsg msg = RpcClientHelper.GetQueueMsg(config, model);
            if (!_Queue.Public(config, msg, serverId))
            {
                throw new ErrorException("rpc.broadcast.error[type=subscribe]");
            }
        }

        public void BroadcastMsg<T> (IRemoteBroadcast config, T model)
        {
            TcpRemoteMsg msg = RpcClientHelper.GetQueueMsg(config, model);
            this._BroadcastMsg(config, msg);
        }
        private void _BroadcastMsg (IRemoteBroadcast config, TcpRemoteMsg msg)
        {
            if (!_Queue.Public(config, msg))
            {
                throw new ErrorException("rpc.broadcast.error[type=subscribe]");
            }
        }
        public bool BroadcastMsg<T> (IRemoteBroadcast config, T model, out string error)
        {
            TcpRemoteMsg msg = RpcClientHelper.GetQueueMsg(config, model);
            if (!_Queue.Public(config, msg))
            {
                error = "rpc.broadcast.error[type=subscribe]";
                return false;
            }
            error = null;
            return true;
        }

        public void BroadcastMsg<T> (IRemoteBroadcast config, T model, string[] typeVal)
        {
            TcpRemoteMsg msg = RpcClientHelper.GetQueueMsg(config, model);
            if (!_Queue.Public(config, msg, typeVal))
            {
                throw new ErrorException("rpc.broadcast.error[type=subscribe]");
            }
        }

        public void Dispose ()
        {
            _Queue?.Dispose();
        }

        public void BroadcastMsg<T> (IRemoteBroadcast config, T model, long rpcMerId, string[] typeVal)
        {
            TcpRemoteMsg msg = RpcClientHelper.GetQueueMsg(config, model);
            if (!_Queue.Public(config, msg, rpcMerId, typeVal))
            {
                throw new ErrorException("rpc.broadcast.error[type=subscribe]");
            }
        }
    }
}
