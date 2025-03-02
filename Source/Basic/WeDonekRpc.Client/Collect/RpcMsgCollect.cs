using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.Ioc;
using WeDonekRpc.Client.Model;
using WeDonekRpc.Client.RpcSysEvent;
using WeDonekRpc.Client.TranService;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Json;
using WeDonekRpc.Model;

namespace WeDonekRpc.Client.Collect
{
    public delegate IBasicRes ASyncMsgEvent (IMsg msg);
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
        internal static void SetMsgEvent (ASyncMsgEvent msg)
        {
            _ASyncMsgEvent = msg;

        }

        private static TcpRemoteReply _GetReply (RemoteMsg msg)
        {
            IBasicRes res = _ASyncMsgEvent(msg);
            if (msg.TcpMsg.IsReply)
            {
                return new TcpRemoteReply(res);
            }
            return null;
        }

        /// <summary>
        /// 消息事件
        /// </summary>
        /// <param name="msg"></param>
        internal static TcpRemoteReply MsgEvent (RemoteMsg msg)
        {
            if (msg.TcpMsg.IsSync)
            {
                using (RemoteLock rLock = SyncLockCollect.ApplyLock(msg.TcpMsg.LockId, msg.TcpMsg.LockType))
                {
                    if (rLock.GetLock())
                    {
                        TcpRemoteReply reply = _MsgEvent(msg);
                        if (reply == null)
                        {
                            rLock.Exit(null);
                        }
                        else if (reply.IsError)
                        {
                            rLock.Exit(reply.MsgBody, true);
                        }
                        else
                        {
                            rLock.Exit(reply.MsgBody);
                        }
                        return reply;
                    }
                    else if (!rLock.IsError && msg.TcpMsg.IsReply)
                    {
                        return new TcpRemoteReply(rLock.Extend);
                    }
                    else if (msg.TcpMsg.IsReply)
                    {
                        return new TcpRemoteReply(new BasicRes(rLock.Error));
                    }
                    return null;
                }
            }
            else
            {
                return _MsgEvent(msg);
            }
        }
        private static TcpRemoteReply _MsgEvent (RemoteMsg msg)
        {
            using (IocScope scope = RpcClient.Ioc.CreateScore())
            {
                _Service.ReceiveMsgEvent(msg);
                TcpRemoteReply reply = _ReplyMsg(msg);
                _Service.ReceiveEndEvent(msg, reply);
                return reply;
            }
        }
        private static TcpRemoteReply _ReplyMsg (RemoteMsg msg)
        {
            if (RemoteSysEvent.MsgEvent(msg, out TcpRemoteReply res))
            {
                return res;
            }
            else if (msg.TcpMsg.Tran != null)
            {
                if (!RpcTranService.AddTranLog(msg, out ICurTranState tran, out string error))
                {
                    return new TcpRemoteReply(new BasicRes(error));
                }
                using (tran)
                {
                    return _GetReply(msg);
                }
            }
            else
            {
                return _GetReply(msg);
            }
        }
        public void BroadcastMsg<T> (IRemoteBroadcast config, T model, long rpcMerId, string[] typeVal)
        {
            BroadcastMsg msg = new BroadcastMsg(config)
            {
                MsgBody = JsonTools.Json(model, model.GetType()),
                TypeVal = typeVal,
                RpcMerId = rpcMerId
            };
            _BroadcastMsg(msg);
        }
        public void BroadcastMsg<T> (IRemoteBroadcast config, T model, string[] typeVal)
        {
            BroadcastMsg msg = new BroadcastMsg(config)
            {
                MsgBody = JsonTools.Json(model, model.GetType()),
                TypeVal = typeVal
            };
            _BroadcastMsg(msg);
        }
        public void BroadcastMsg (IRemoteBroadcast config, DynamicModel body)
        {
            BroadcastMsg msg = new BroadcastMsg(config)
            {
                MsgBody = body.Json()
            };
            _BroadcastMsg(msg);
        }

        public void BroadcastMsg<T> (IRemoteBroadcast config, T model, long[] serverId)
        {
            BroadcastMsg msg = new BroadcastMsg(config)
            {
                MsgBody = JsonTools.Json(model, model.GetType()),
                ServerId = serverId
            };
            _BroadcastMsg(msg);
        }
        public void BroadcastMsg<T> (IRemoteBroadcast config, T model)
        {
            BroadcastMsg msg = new BroadcastMsg(config)
            {
                MsgBody = JsonTools.Json(model, model.GetType())
            };
            _BroadcastMsg(msg);
        }
        public bool BroadcastMsg<T> (IRemoteBroadcast config, T model, out string error)
        {
            BroadcastMsg msg = new BroadcastMsg(config)
            {
                MsgBody = JsonTools.Json(model, model.GetType())
            };
            return !RemoteCollect.Send(msg, out error);
        }

        private static void _BroadcastMsg (BroadcastMsg msg)
        {
            if (!RemoteCollect.Send(msg, out string error))
            {
                throw new ErrorException(error);
            }
        }
    }
}
