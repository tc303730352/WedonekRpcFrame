using WeDonekRpc.Client;

namespace RpcStore.RemoteModel.Identity
{
    /// <summary>
    /// 设置身份标识启用状态
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class SetIdentityIsEnable : RpcRemote
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
