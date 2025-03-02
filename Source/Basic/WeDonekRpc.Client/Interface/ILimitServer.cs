using System;
using WeDonekRpc.IOSendInterface;
using WeDonekRpc.Model;

namespace WeDonekRpc.Client.Interface
{
    internal interface ILimitServer : IDisposable
    {
        void AddEntrust(Action action);
        TcpRemoteReply MsgEvent(string key, TcpRemoteMsg msg, IIOClient client);
    }
}