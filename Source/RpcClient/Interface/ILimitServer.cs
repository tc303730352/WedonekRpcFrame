using System;

using RpcModel;

using SocketTcpServer.Interface;

namespace RpcClient.Interface
{
        internal interface ILimitServer : IDisposable
        {
                void AddEntrust(Action action);
                TcpRemoteReply MsgEvent(string key, TcpRemoteMsg msg, ISocketClient client);
        }
}