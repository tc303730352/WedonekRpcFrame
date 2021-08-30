using System;
using System.Collections.Generic;

using RpcClient.Model;

namespace RpcClient.Interface
{
        public delegate void ReceiveMsgEvent(IMsg msg);

        public delegate void SendMsgEvent(string dictate, ref Dictionary<string, string> extend);

        public interface IRpcService
        {
                event ReceiveMsgEvent ReceiveMsg;
                event SendMsgEvent SendMsg;
                event Action StartUpComplate;
        }
}
