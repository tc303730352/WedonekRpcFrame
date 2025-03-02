using System;
using WeDonekRpc.Client.Collect;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.Model;
using WeDonekRpc.IOSendInterface;
using WeDonekRpc.Model;
using WeDonekRpc.Model.Model;

namespace WeDonekRpc.Client.Limit
{
    internal class PublicLimit : IMsgLimit
    {
        private readonly IServerLimit _Limit;
        private readonly bool _IsLimit = true;
        private readonly LeakLimit _Bucket;

        public PublicLimit (ServerLimitConfig config, Action<LeakRemoteMsg> send, ILimitServer server)
        {
            if (config.LimitType == ServerLimitType.固定时间窗)
            {
                this._Limit = new FixedTimeLimit(config.LimitNum, config.LimitTime);
            }
            else if (config.LimitType == ServerLimitType.流动时间窗)
            {
                this._Limit = new SlideTimeLimit(config.LimitNum, config.LimitTime);
            }
            else if (config.LimitType == ServerLimitType.令牌桶)
            {
                this._Limit = new TokenLimit(config.TokenNum, config.TokenInNum);
            }
            else
            {
                this._IsLimit = false;
            }
            if (config.IsEnableBucket || config.LimitType == ServerLimitType.漏桶)
            {
                this._Bucket = new LeakLimit(config.BucketSize, config.BucketOutNum, send, server);
            }
        }
        public bool IsInvalid => false;

        public bool IsUsable => this._CheckIsUsable();

        private bool _CheckIsUsable ()
        {
            if (this._IsLimit && this._Limit.IsUsable)
            {
                return true;
            }
            else if (this._Bucket != null)
            {
                return this._Bucket.IsUsable;
            }
            else if (this._IsLimit)
            {
                return false;
            }
            return true;
        }
        public bool IsLimit ()
        {
            if (this._IsLimit)
            {
                return this._Limit.IsLimit();
            }
            return false;
        }

        public TcpRemoteReply MsgEvent (string key, TcpRemoteMsg msg, IIOClient client)
        {
            if (this._IsLimit && !this._Limit.IsLimit())
            {
                return RpcMsgCollect.MsgEvent(new Model.RemoteMsg(key, msg));
            }
            else if (this._Bucket != null)
            {
                return this._Bucket.MsgEvent(key, msg, client);
            }
            else if (this._IsLimit)
            {
                return new TcpRemoteReply(new BasicRes("rpc.exceed.limt"));
            }
            return RpcMsgCollect.MsgEvent(new Model.RemoteMsg(key, msg));
        }

        public void Refresh (int time)
        {
            if (this._IsLimit)
            {
                this._Limit.Refresh(time);
            }
            this._Bucket?.Refresh(time);
        }

        public void Reset ()
        {
            if (this._IsLimit)
            {
                this._Limit.Reset();
            }
            this._Bucket?.Reset();
        }
    }
}
