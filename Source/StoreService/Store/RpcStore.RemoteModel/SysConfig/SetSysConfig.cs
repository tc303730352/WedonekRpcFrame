using WeDonekRpc.Client;
using RpcStore.RemoteModel.SysConfig.Model;

namespace RpcStore.RemoteModel.SysConfig
{
    /// <summary>
    /// 修改配置
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class SetSysConfig : RpcRemote
    {
        /// <summary>
        /// 数据ID
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 配置值
        /// </summary>
        public SysConfigSet ConfigSet
        {
            get;
            set;
        }
    }
}
