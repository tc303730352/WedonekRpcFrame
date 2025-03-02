using WeDonekRpc.Client;
using WeDonekRpc.Model;

namespace WeDonekRpc.ModularModel.SysEvent.Msg
{
    [IRemoteBroadcast("RefreshRpcEvent")]
    public class RefreshRpcEvent : RpcBroadcast
    {
        public string Module { get; set; }
        public long RpcMerId { get; set; }
    }
}
