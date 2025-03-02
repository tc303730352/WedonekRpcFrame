using WeDonekRpc.Client;

namespace RpcStore.RemoteModel.IpBlack
{
    /// <summary>
    /// 修改Ip黑名单备注
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class SetIpBlackRemark : RpcRemote
    {
        /// <summary>
        /// 数据ID
        /// </summary>
        public long Id
        {
            get;
            set;
        }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get;
            set;
        }
    }
}
