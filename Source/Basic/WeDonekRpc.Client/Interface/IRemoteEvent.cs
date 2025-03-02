using WeDonekRpc.Client.Model;

using WeDonekRpc.Model;

namespace WeDonekRpc.Client.Interface
{
        internal interface IRemoteEvent
        {
                TcpRemoteReply MsgEvent(RemoteMsg msg);
        }
}