using WeDonekRpc.Client;
using WeDonekRpc.Model;

namespace WeDonekRpc.ModularModel.Shield.Msg
{
    [IRemoteBroadcast("RefreshApiShieId", IsOnly = false)]
    public class RefreshApiShieId : RpcBroadcast
    {
        public string Path
        {
            get;
            set;
        }
    }
}
