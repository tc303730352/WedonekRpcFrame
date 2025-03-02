using WeDonekRpc.Client;

namespace RpcStore.RemoteModel.ServerType
{
    /// <summary>
    /// 删除类别
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class DeleteServerType:RpcRemote
    {
        /// <summary>
        /// 数据ID
        /// </summary>
        public long Id { get; set; }
    }
}
