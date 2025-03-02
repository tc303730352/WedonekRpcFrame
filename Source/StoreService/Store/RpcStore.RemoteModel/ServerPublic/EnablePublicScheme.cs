using WeDonekRpc.Client;

namespace RpcStore.RemoteModel.ServerPublic
{
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class EnablePublicScheme : RpcRemote<bool>
    {
        public long Id { get; set; }
    }
}
