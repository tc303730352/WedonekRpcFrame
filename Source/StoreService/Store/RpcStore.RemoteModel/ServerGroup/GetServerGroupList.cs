using WeDonekRpc.Client;
using WeDonekRpc.Model;
using RpcStore.RemoteModel.ServerGroup.Model;

namespace RpcStore.RemoteModel.ServerGroup
{
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class GetServerGroupList : RpcRemoteArray<ServerGroupList>
    {
        public RpcServerType? ServerType { get; set; }
    }
}
