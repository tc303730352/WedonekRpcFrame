using System;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.Ioc;
using WeDonekRpc.Client.Log;
using WeDonekRpc.Client.Model;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;

namespace WeDonekRpc.Client.RpcSysEvent
{

    internal class RemoteEvent : IRemoteEvent
    {
        private static readonly IIocService _Ioc = RpcClient.Ioc;
        public RemoteEvent (Func<MsgSource, TcpRemoteReply> action)
        {
            this._Action = action;
        }
        private readonly Func<MsgSource, TcpRemoteReply> _Action = null;
        public TcpRemoteReply MsgEvent (RemoteMsg msg)
        {
            using (IocScope scope = _Ioc.CreateScore())
            {
                try
                {
                    return this._Action(msg.Source);
                }
                catch (Exception e)
                {
                    ErrorException ex = ErrorException.FormatError(e);
                    RpcLogSystem.AddErrorLog(this._Action.Method, msg, ex);
                    return new TcpRemoteReply(new BasicRes(ex.ToString()));
                }
            }
        }
    }
}
