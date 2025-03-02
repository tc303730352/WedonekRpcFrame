using WeDonekRpc.Client;
using RpcStore.RemoteModel.SysConfig.Model;

namespace RpcStore.RemoteModel.SysConfig
{
    /// <summary>
    /// 新增配置
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class AddSysConfig : RpcRemote
    {
        /// <summary>
        /// 配置资料
        /// </summary>
        public SysConfigAdd Config { get; set; }
    }
}
