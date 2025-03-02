using WeDonekRpc.Client;
using RpcStore.RemoteModel.ServerBind.Model;

namespace RpcStore.RemoteModel.ServerBind
{
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class GetServerBindItems : RpcRemoteArray<BindServerItem>
    {
        public ServerBindQueryParam Query { get; set; }
    }
}
