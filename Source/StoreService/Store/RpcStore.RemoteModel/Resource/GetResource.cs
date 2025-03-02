using WeDonekRpc.Client;
using RpcStore.RemoteModel.Resource.Model;

namespace RpcStore.RemoteModel.Resource
{
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class GetResource : RpcRemote<ResourceDto>
    {
        public long Id { get; set; }
    }
}
