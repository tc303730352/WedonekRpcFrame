using WeDonekRpc.Client;

namespace RpcStore.RemoteModel.IpBlack
{
    /// <summary>
    /// 删除Ip黑名单
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class DropIpBack : RpcRemote
    {
        /// <summary>
        /// 黑名单ID
        /// </summary>
        public long Id
        {
            get;
            set;
        }
    }
}
