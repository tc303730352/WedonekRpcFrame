using WeDonekRpc.Client;
using RpcStore.RemoteModel.ClientLimit.Model;

namespace RpcStore.RemoteModel.ClientLimit
{
    /// <summary>
    /// 添加或设置服务节点限流配置
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class SyncClientLimit : RpcRemote
    {
        /// <summary>
        /// 限流配置
        /// </summary>
        public ClientLimitDatum Datum
        {
            get;
            set;
        }
    }
}
