using WeDonekRpc.IOSendInterface;
using WeDonekRpc.Model;

namespace WeDonekRpc.Client.Interface
{
    internal interface IMsgLimit : IServerLimit
    {
        TcpRemoteReply MsgEvent (string key, TcpRemoteMsg msg, IIOClient client);
    }
}