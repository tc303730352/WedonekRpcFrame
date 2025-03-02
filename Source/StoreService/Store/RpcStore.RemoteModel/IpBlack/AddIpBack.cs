using WeDonekRpc.Client;
using RpcStore.RemoteModel.IpBlack.Model;

namespace RpcStore.RemoteModel.IpBlack
{
    /// <summary>
    /// 添加Ip黑名单
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class AddIpBack : RpcRemote<long>
    {
        /// <summary>
        /// Ip黑名单
        /// </summary>
        public IpBlackAdd Datum
        {
            get;
            set;
        }
    }
}
