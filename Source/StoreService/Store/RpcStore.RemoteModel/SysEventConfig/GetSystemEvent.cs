using WeDonekRpc.Client;

namespace RpcStore.RemoteModel.SysEventConfig
{
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class GetSystemEvent : RpcRemote<Model.SystemEventConfig>
    {
        public int Id { get; set; }
    }
}
