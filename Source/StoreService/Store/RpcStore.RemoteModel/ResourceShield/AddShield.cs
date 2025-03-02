using WeDonekRpc.Client;
using RpcStore.RemoteModel.ResourceShield.Model;

namespace RpcStore.RemoteModel.ResourceShield
{
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class AddShield : RpcRemote
    {
        public ShieldAddDatum Datum { get; set; }
    }
}
