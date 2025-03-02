using WeDonekRpc.Client;

namespace RpcStore.RemoteModel.SysConfig
{
    /// <summary>
    /// 设置配置的启用状态
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class SetSysConfigIsEnable : RpcRemote
    {
        /// <summary>
        /// 数据ID
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnable { get; set; }
    }
}
