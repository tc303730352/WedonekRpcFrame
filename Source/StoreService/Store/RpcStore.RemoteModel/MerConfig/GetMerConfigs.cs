using WeDonekRpc.Client;

namespace RpcStore.RemoteModel.MerConfig
{
    /// <summary>
    /// 获取集群下的所有配置
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class GetMerConfigByMerId : RpcRemoteArray<Model.RpcMerConfigDatum>
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
