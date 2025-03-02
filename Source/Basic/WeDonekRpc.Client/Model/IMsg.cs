using System;
using System.Collections.Generic;

using WeDonekRpc.Model;
namespace WeDonekRpc.Client.Model
{
    public interface IMsg
    {
        uint MsgId { get; }
        Dictionary<string, string> Extend { get; }
        string MsgKey
        {
            get;
        }
        byte[] Stream { get; }
        string MsgBody
        {
            get;
        }
        T GetMsgBody<T>();

        object GetMsgBody(Type type);

        /// <summary>
        /// 消息来源
        /// </summary>
        MsgSource Source
        {
            get;
        }

    }
}
