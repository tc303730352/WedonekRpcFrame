using WeDonekRpc.Client;

namespace RpcStore.RemoteModel.SysConfig
{
    /// <summary>
    /// 删除配置
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class DeleteSysConfig : RpcRemote
    {
        /// <summary>
        /// 数据ID
        /// </summary>
        public long Id { get; set; }
    }
}
