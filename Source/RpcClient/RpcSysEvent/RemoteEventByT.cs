using System;

using RpcClient.Interface;
using RpcClient.Model;

using RpcModel;

using RpcHelper;

namespace RpcClient.RpcSysEvent
{
        internal class RemoteEvent<T> : IRemoteEvent
        {
                private static readonly TcpRemoteReply _ErrorRes = new TcpRemoteReply(new BasicRes("public.system.error"));
                public RemoteEvent(Func<T, TcpRemoteReply> action)
                {
                        this._Action = action;
                }
                private readonly Func<T, TcpRemoteReply> _Action = null;
                public TcpRemoteReply MsgEvent(RemoteMsg msg)
                {
                        T obj = msg.GetMsgBody<T>();
                        try
                        {
                                return this._Action(obj);
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
