using WeDonekRpc.Client;

namespace RpcStore.RemoteModel.Mer
{
    /// <summary>
    /// 删除服务集群
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class DeleteMer : RpcRemote
    {
        /// <summary>
        /// 集群ID
        /// </summary>
        public long RpcMerId
        {
            get;
            set;
        }
    }
}
