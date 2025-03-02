using WeDonekRpc.Client;

namespace RpcStore.RemoteModel.ClientLimit
{
    /// <summary>
    /// 删除服务节点限流配置
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class DeleteClientLimit : RpcRemote
    {
        /// <summary>
        /// 数据ID
        /// </summary>
        public long Id
        {
            get;
            set;
        }
    }
}
