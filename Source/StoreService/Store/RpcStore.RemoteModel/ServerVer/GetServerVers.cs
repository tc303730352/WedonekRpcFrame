using RpcStore.RemoteModel.ServerVer.Model;
using WeDonekRpc.Client;

namespace RpcStore.RemoteModel.ServerVer
{
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class GetServerVers : RpcRemoteArray<ServerVerInfo>
    {
        public long RpcMerId { get; set; }
    }
}
