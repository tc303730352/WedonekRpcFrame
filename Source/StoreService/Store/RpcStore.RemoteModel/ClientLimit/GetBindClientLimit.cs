using RpcStore.RemoteModel.ClientLimit.Model;
using WeDonekRpc.Client;

namespace RpcStore.RemoteModel.ClientLimit
{
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class GetBindClientLimit : RpcRemoteArray<ClientLimitModel>
    {
        public long ServerId { get; set; }
    }
}
