using WeDonekRpc.Client;

namespace RpcStore.RemoteModel.ServerGroup
{
    /// <summary>
    /// 删除服务组
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class DeleteServerGroup:RpcRemote
    {
        /// <summary>
        /// 数据ID
        /// </summary>
        public long Id { get; set; }
    }
}
