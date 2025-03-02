using WeDonekRpc.Client;
using RpcStore.RemoteModel.SysConfig.Model;

namespace RpcStore.RemoteModel.SysConfig
{
    /// <summary>
    /// 获取系统配置
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class GetSysConfig : RpcRemote<SysConfigDatum>
    {
        /// <summary>
        /// 配置ID
        /// </summary>
        public long Id { get; set; }
    }
}
