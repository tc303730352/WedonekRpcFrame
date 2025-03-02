using WeDonekRpc.Client;

namespace RpcStore.RemoteModel.IpBlack
{
    /// <summary>
    /// 修改Ip黑名单
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class SetIpBlack : RpcRemote
    {
        /// <summary>
        /// Ip黑名单ID
        /// </summary>
        public long Id
        {
            get;
            set;
        }
        /// <summary>
        /// 黑名单资料
        /// </summary>
        public Model.IpBlackSet Datum
        {
            get;
            set;
        }
    }
}
