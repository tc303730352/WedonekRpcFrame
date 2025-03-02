using WeDonekRpc.Client;
using RpcStore.RemoteModel.Mer.Model;

namespace RpcStore.RemoteModel.Mer
{
    /// <summary>
    /// 修改服务集群资料
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class SetMer : RpcRemote
    {
        /// <summary>
        /// 集群ID
        /// </summary>
        public long RpcMerId
        {
            get;
            set;
        }
        /// <summary>
        /// 集群资料
        /// </summary>
        public RpcMerSet Datum
        {
            get;
            set;
        }
    }
}
