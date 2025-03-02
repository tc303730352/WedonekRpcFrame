using WeDonekRpc.Client;

namespace RpcStore.RemoteModel.ServerBind
{
    /// <summary>
    /// 删除集群绑定的服务节点关系
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class DeleteServerBind : RpcRemote
    {
        /// <summary>
        /// 数据ID
        /// </summary>
        public long Id { get; set; }
    }
}
