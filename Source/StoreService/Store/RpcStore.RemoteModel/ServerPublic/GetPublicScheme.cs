using WeDonekRpc.Client;
using RpcStore.RemoteModel.ServerPublic.Model;

namespace RpcStore.RemoteModel.ServerPublic
{
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class GetPublicScheme : RpcRemote<PublicScheme>
    {
        public long Id {  get; set; }
    }
}
