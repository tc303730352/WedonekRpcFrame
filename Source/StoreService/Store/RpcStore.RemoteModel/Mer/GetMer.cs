using WeDonekRpc.Client;
using RpcStore.RemoteModel.Mer.Model;

namespace RpcStore.RemoteModel.Mer
{
    /// <summary>
    /// 获取服务集群资料
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class GetMer : RpcRemote<RpcMerDatum>
    {
        /// <summary>
        /// 服务集群ID
        /// </summary>
        public long RpcMerId
        {
            get;
            set;
        }
    }
}
