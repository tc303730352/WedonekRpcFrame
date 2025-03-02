using WeDonekRpc.IOSendInterface;
using WeDonekRpc.Client.Collect;
using WeDonekRpc.Client.Interface;

using WeDonekRpc.Model;

namespace WeDonekRpc.Client.Limit
{
    internal class NoEnableLimit : IMsgLimit
    {
        public bool IsUsable => true;
        public bool IsInvalid => false;
        public bool IsLimit()
        {
            return false;
        }

        public TcpRemoteReply MsgEvent(string key, TcpRemoteMsg msg, IIOClient client)
        {
            return RpcMsgCollect.MsgEvent(new Model.RemoteMsg(key, msg));
        }

        public void Refresh(int time)
        {

        }

        public void Reset()
        {

        }
    }
}
