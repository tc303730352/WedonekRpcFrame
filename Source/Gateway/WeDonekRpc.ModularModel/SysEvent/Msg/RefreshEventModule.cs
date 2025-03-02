using WeDonekRpc.Client;
using WeDonekRpc.Model;

namespace WeDonekRpc.ModularModel.SysEvent.Msg
{
    [IRemoteBroadcast("RefreshEventModule")]
    public class RefreshEventModule : RpcBroadcast
    {
        public string Module { get; set; }
    }
}
