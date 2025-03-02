using WeDonekRpc.Client;

namespace RpcStore.RemoteModel.ServerPublic
{
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class DeletePublicScheme : RpcRemote
    {
        public long Id { get; set; }
    }
}
