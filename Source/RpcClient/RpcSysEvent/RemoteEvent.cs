using System;

using RpcClient.Interface;
using RpcClient.Model;

using RpcModel;

using RpcHelper;

namespace RpcClient.RpcSysEvent
{

        internal class RemoteEvent : IRemoteEvent
        {
                public RemoteEvent(Func<TcpRemoteReply> action)
                {
                        this._Action = action;
                }
                private readonly Func<TcpRemoteReply> _Action = null;
                public TcpRemoteReply MsgEvent(RemoteMsg msg)
                {
                        try
                        {
                                return this._Action();
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
