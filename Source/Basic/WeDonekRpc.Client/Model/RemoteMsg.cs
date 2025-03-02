using System;
using System.Collections.Generic;
using WeDonekRpc.Helper.Json;
using WeDonekRpc.Model;

namespace WeDonekRpc.Client.Model
{
    public class RemoteMsg : IMsg
    {

        public RemoteMsg (string key, TcpRemoteMsg msg)
        {
            this.MsgKey = key;
            this.TcpMsg = msg;
        }

        internal TcpRemoteMsg TcpMsg
        {
            get;
        }

        public string MsgKey
        {
            get;
        }
        public T GetMsgBody<T> ()
        {
            return JsonTools.Json<T>(this.TcpMsg.MsgBody);
        }

        public object GetMsgBody (Type type)
        {
            return JsonTools.Json(this.TcpMsg.MsgBody, type);
        }

        public MsgSource Source => this.TcpMsg.Source;

        public string MsgBody => this.TcpMsg.MsgBody;

        public Dictionary<string, string> Extend => this.TcpMsg.Extend;

        public byte[] Stream => this.TcpMsg.Stream;

        public uint MsgId => this.TcpMsg.PageId;
    }
}
