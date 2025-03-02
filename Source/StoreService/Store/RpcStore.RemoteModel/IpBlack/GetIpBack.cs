using WeDonekRpc.Client;
using RpcStore.RemoteModel.IpBlack.Model;

namespace RpcStore.RemoteModel.IpBlack
{
    /// <summary>
    /// 获取Ip黑名单
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class GetIpBack : RpcRemote<IpBlackDatum>
    {
        /// <summary>
        /// 数据ID
        /// </summary>
        public long Id { get; set; }
    }
}
