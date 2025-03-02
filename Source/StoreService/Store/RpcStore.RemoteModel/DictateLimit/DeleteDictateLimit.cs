using WeDonekRpc.Client;

namespace RpcStore.RemoteModel.DictateLimit
{
    /// <summary>
    /// 删除指令限流配置
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class DeleteDictateLimit : RpcRemote
    {
        /// <summary>
        /// 数据ID
        /// </summary>
        public long Id { get; set; }
    }
}
