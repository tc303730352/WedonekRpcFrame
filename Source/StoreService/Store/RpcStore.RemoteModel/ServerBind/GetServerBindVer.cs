using RpcStore.RemoteModel.ServerBind.Model;
using WeDonekRpc.Client;

namespace RpcStore.RemoteModel.ServerBind
{
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class GetServerBindVer : RpcRemoteArray<ServerBindVer>
    {
        public long RpcMerId { get; set; }
        public bool? IsHold { get; set; }
    }
}
