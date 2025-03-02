using WeDonekRpc.Client;

namespace RpcStore.RemoteModel
{
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class CheShiFunc : RpcRemote<string>
    {
        public string Id { get; set; }
    }
}
