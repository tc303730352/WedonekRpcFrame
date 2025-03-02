using WeDonekRpc.Client;

namespace RpcStore.RemoteModel.Control
{
    /// <summary>
    /// 删除服务中心
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class DeleteControl : RpcRemote
    {
        /// <summary>
        /// 服务中心ID
        /// </summary>
        public int Id { get; set; }
    }
}
