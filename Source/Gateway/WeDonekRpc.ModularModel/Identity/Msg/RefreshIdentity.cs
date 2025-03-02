using WeDonekRpc.Client;
using WeDonekRpc.Model;

namespace WeDonekRpc.ModularModel.Identity.Msg
{
    [IRemoteBroadcast(true, "sys.extend")]
    public class RefreshIdentity : RpcBroadcast
    {
        public string AppId
        {
            get;
            set;
        }
    }
}
