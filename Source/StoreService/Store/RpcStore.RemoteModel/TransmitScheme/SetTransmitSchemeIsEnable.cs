using WeDonekRpc.Client;

namespace RpcStore.RemoteModel.TransmitScheme
{
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class SetTransmitSchemeIsEnable : RpcRemote
    {
        public long Id { get; set; }

        public bool IsEnable { get; set; }
    }
}
