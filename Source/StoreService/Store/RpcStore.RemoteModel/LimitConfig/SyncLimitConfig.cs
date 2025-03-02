using WeDonekRpc.Client;
using RpcStore.RemoteModel.LimitConfig.Model;

namespace RpcStore.RemoteModel.LimitConfig
{
    /// <summary>
    /// 同步服务节点全局限流配置(添加或修改)
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class SyncLimitConfig : RpcRemote
    {
        /// <summary>
        /// 全局限流配置
        /// </summary>
        public LimitConfigDatum Config
        {
            get;
            set;
        }
    }
}
