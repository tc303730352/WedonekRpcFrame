using WeDonekRpc.IOSendInterface;
using WeDonekRpc.Client.Collect;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.Log;
using WeDonekRpc.Client.Model;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;
using WeDonekRpc.Model.Model;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace WeDonekRpc.Client.Limit
{
    internal class ServerLimit : ILimitServer
    {
        private readonly Dictionary<string, IMsgLimit> _Dictate = new Dictionary<string, IMsgLimit>();
        private Timer _RefreshTime = null;
        private Timer _MsgTrigger = null;
        private Action[] _MsgEntrust = null;
        private IMsgLimit _Limit = null;
        private bool _IsLimit = true;

        public ServerLimit(LimitConfigRes config)
        {
            this._Init(config.LimitConfig, config.DictateLimit);
        }
        /// <summary>
        /// 限流配置
        /// </summary>
        private void _Init(ServerLimitConfig config, ServerDictateLimit[] dictates)
        {
            if (!config.IsEnable && dictates.IsNull())
            {
                this._IsLimit = false;
                return;
            }
            if (config.IsEnable)
            {
                this._Limit = new PublicLimit(config, _SendMsg, this);
            }
            if (!dictates.IsNull())
            {
                dictates.ForEach(a =>
                {
                    this._Dictate.Add(a.Dictate, _GetMsgLimit(a, this));
                });
            }
            this._RefreshTime = new Timer(this._Refresh, null, 1000, 1000);
            if (!this._MsgEntrust.IsNull())
            {
                this._MsgTrigger = new Timer(this._Send, null, 10, 10);
            }
        }
        private void _Send(object state)
        {
            if (this._MsgEntrust.Length < 5)
            {
                this._MsgEntrust.ForEach(a => a.Invoke());
                return;
            }
            Parallel.ForEach(this._MsgEntrust, a => a.Invoke());
        }
        private void _Refresh(object state)
        {
            int now = HeartbeatTimeHelper.HeartbeatTime;
            if (this._Limit != null)
            {
                this._Limit.Refresh(now);
            }
            foreach (IMsgLimit limit in this._Dictate.Values)
            {
                limit.Refresh(now);
            }
        }
        private static IMsgLimit _GetMsgLimit(ServerDictateLimit limit, ILimitServer server)
        {
            if (limit.LimitType == ServerLimitType.令牌桶)
            {
                return new TokenLimit(limit.TokenNum, limit.TokenInNum);
            }
            else if (limit.LimitType == ServerLimitType.固定时间窗)
            {
                return new FixedTimeLimit(limit.LimitNum, limit.LimitTime);
            }
            else if (limit.LimitType == ServerLimitType.流动时间窗)
            {
                return new SlideTimeLimit(limit.LimitNum, limit.LimitTime);
            }
            else if (limit.LimitType == ServerLimitType.漏桶)
            {
                return new LeakLimit(limit.BucketSize, limit.BucketOutNum, _SendMsg, server);
            }
            else
            {
                return new NoEnableLimit();
            }
        }
        private static void _SendMsg(LeakRemoteMsg msg)
        {
            TcpRemoteReply reply = RpcMsgCollect.MsgEvent(msg.Msg);
            if (!msg.Client.ReplyMsg(reply, out string error))
            {
                RpcLogSystem.AddMsgErrorLog(msg.Msg, reply, error);
            }
        }
        public void AddEntrust(Action action)
        {
            this._MsgEntrust = this._MsgEntrust.Add(action);
        }


        public TcpRemoteReply MsgEvent(string key, TcpRemoteMsg msg, IIOClient client)
        {
            if (!this._IsLimit)
            {
                return RpcMsgCollect.MsgEvent(new Model.RemoteMsg(key, msg));
            }
            else if (this._Dictate.TryGetValue(key, out IMsgLimit limit))
            {
                return limit.MsgEvent(key, msg, client);
            }
            else if (this._Limit != null)
            {
                return this._Limit.MsgEvent(key, msg, client);
            }
            return RpcMsgCollect.MsgEvent(new Model.RemoteMsg(key, msg));
        }

        public void Dispose()
        {
            this._RefreshTime?.Dispose();
            this._MsgTrigger?.Dispose();
        }
    }
}
