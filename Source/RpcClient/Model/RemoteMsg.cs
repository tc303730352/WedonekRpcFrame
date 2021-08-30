using System;
using System.Collections.Generic;

using RpcModel;

namespace RpcClient.Model
{
        public class RemoteMsg : IMsg
        {
                public RemoteMsg(string key, TcpRemoteMsg msg)
                {
                        this.MsgKey = key;
                        this.TcpMsg = msg;
                }

                public RemoteMsg(QueueRemoteMsg data)
                {
                        this.MsgKey = data.Type;
                        this.TcpMsg = data.Msg;
                }

                internal TcpRemoteMsg TcpMsg
                {
                        get;
                }

                public string MsgKey
                {
                        get;
                }
                public T GetMsgBody<T>()
                {
                        return RpcHelper.Tools.Json<T>(this.TcpMsg.MsgBody);
                }

                public object GetMsgBody(Type type)
                {
                        return RpcHelper.Tools.Json(this.TcpMsg.MsgBody, type);
                }

                public MsgSource Source => this.TcpMsg.Source;

                public string MsgBody => this.TcpMsg.MsgBody;

                public Dictionary<string, string> Extend => this.TcpMsg.Extend;
        }
}
