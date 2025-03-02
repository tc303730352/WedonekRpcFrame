using System;
using WeDonekRpc.Client.Helper;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.Ioc;
using WeDonekRpc.Client.Model;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;

namespace WeDonekRpc.Client.Collect
{
    internal class RpcQueueCollect : IBroadcast
    {
        /// <summary>
        /// 同步的消息事件
        /// </summary>
        private static Func<IMsg, bool> _MsgEvent = null;
        /// <summary>
        /// 数据队列
        /// </summary>
        private static IQueueCollect _Queue = null;

        public static void SetMsgEvent (Func<IMsg, bool> msgEvent)
        {
            _MsgEvent = msgEvent;
        }

        public static void InitQueue ()
        {
            QueueCollect.InitQueue();
            _Queue = QueueCollect.CreateMsgQueue(_QueueMsg);
            _Queue.Subscribe();
        }
        /// <summary>
        /// 消息队列
        /// </summary>
        /// <param name="data"></param>
        /// <param name="routeKey"></param>
        /// <param name="exchange"></param>
        /// <returns></returns>
        private static SubscribeErrorType _QueueMsg (QueueRemoteMsg data, string routeKey, string exchange)
        {
            if (data.Msg.Source.ServerId == RpcClient.CurrentSource.ServerId && data.IsExclude)
            {
                return SubscribeErrorType.Exclude;
            }
            TcpRemoteMsg msg = data.Msg;
            if (data.Msg.IsSync)
            {
                using (RemoteLock rLock = SyncLockCollect.ApplyLock(msg.LockId, msg.LockType))
                {
                    if (rLock.GetLock())
                    {
                        using (IocScope scope = RpcClient.Ioc.CreateScore())
                        {
                            if (_MsgEvent(new RemoteMsg(data.Type, msg)))
                            {
                                rLock.Exit(null);
                                return SubscribeErrorType.Success;
                            }
                            else
                            {
                                rLock.SetError("rpc.queue.msg.exec.fail");
                                return SubscribeErrorType.Fail;
                            }
                        }
                    }
                    else
                    {
                        return rLock.IsError ? SubscribeErrorType.Success : SubscribeErrorType.Fail;
                    }
                }
            }
            else
            {
                using (IocScope scope = RpcClient.Ioc.CreateScore())
                {
                    return _MsgEvent(new RemoteMsg(data.Type, msg)) ? SubscribeErrorType.Success : SubscribeErrorType.Fail;
                }
            }
        }


        public void BroadcastMsg (IRemoteBroadcast config, DynamicModel body)
        {
            TcpRemoteMsg msg = RpcClientHelper.GetQueueMsg(config, body);
            if (!_Queue.Public(config, msg))
            {
                throw new ErrorException("rpc.broadcast.error[type=queue]");
            }
        }

        public void BroadcastMsg<T> (IRemoteBroadcast config, T model, long[] serverId)
        {
            TcpRemoteMsg msg = RpcClientHelper.GetQueueMsg(config, model);
            if (!_Queue.Public(config, msg, serverId))
            {
                throw new ErrorException("rpc.broadcast.error[type=queue]");
            }
        }

        public void BroadcastMsg<T> (IRemoteBroadcast config, T model)
        {
            TcpRemoteMsg msg = RpcClientHelper.GetQueueMsg(config, model);
            if (!_Queue.Public(config, msg))
            {
                throw new ErrorException("rpc.broadcast.error[type=queue]");
            }
        }
        public bool BroadcastMsg<T> (IRemoteBroadcast config, T model, out string error)
        {
            TcpRemoteMsg msg = RpcClientHelper.GetQueueMsg(config, model);
            if (!_Queue.Public(config, msg))
            {
                error = "rpc.broadcast.error[type=queue]";
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
                throw new ErrorException("rpc.broadcast.error[type=queue]");
            }
        }

        internal static void Dispose ()
        {
            _Queue?.Dispose();
        }

        public void BroadcastMsg<T> (IRemoteBroadcast config, T model, long rpcMerId, string[] typeVal)
        {
            TcpRemoteMsg msg = RpcClientHelper.GetQueueMsg(config, model);
            if (!_Queue.Public(config, msg, rpcMerId, typeVal))
            {
                throw new ErrorException("rpc.broadcast.error");
            }
        }
    }
}
