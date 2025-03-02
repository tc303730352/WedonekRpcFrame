using WeDonekRpc.Client;
using WeDonekRpc.Model;

namespace WeDonekRpc.ModularModel.Shield.Msg
{
    [IRemoteBroadcast("RefreshRpcShieId", IsOnly = false)]
    public class RefreshRpcShieId : RpcBroadcast
    {
        public string SysDictate
        {
            get;
            set;
        }
    }
}
