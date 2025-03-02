using WeDonekRpc.Client;

namespace RpcStore.RemoteModel.ServerConfig
{
    /// <summary>
    /// 设置服务节点版本号
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class SetServerVerNum : RpcRemote
    {
        public long Id { get; set; }

        public int VerNum { get; set; }
    }
}
