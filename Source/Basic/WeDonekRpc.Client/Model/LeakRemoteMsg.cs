using WeDonekRpc.IOSendInterface;

namespace WeDonekRpc.Client.Model
{
        internal class LeakRemoteMsg
        {
                public RemoteMsg Msg { get; set; }
                public IIOClient Client { get; set; }
                public int ExpireTime { get; set; }
        }
}
