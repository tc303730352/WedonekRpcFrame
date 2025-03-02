using WeDonekRpc.Client;
using RpcStore.RemoteModel.Mer.Model;

namespace RpcStore.RemoteModel.Mer
{
    /// <summary>
    /// 添加服务集群
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class AddMer : RpcRemote<long>
    {
        /// <summary>
        /// 集群资料
        /// </summary>
        public RpcMerAdd Datum
        {
            get;
            set;
        }
    }
}
