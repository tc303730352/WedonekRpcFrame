using WeDonekRpc.Client;

namespace RpcStore.RemoteModel.ResourceModular
{
    /// <summary>
    /// 设置资源模块备注信息
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class SetModularRemark : RpcRemote
    {
        /// <summary>
        /// 数据ID
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
