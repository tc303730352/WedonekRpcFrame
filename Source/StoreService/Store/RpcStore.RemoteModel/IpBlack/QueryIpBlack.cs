using WeDonekRpc.Client;
using RpcStore.RemoteModel.IpBlack.Model;

namespace RpcStore.RemoteModel.IpBlack
{
    /// <summary>
    /// 查询Ip黑名单
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class QueryIpBlack : BasicPage<Model.IpBlack>
    {
        /// <summary>
        /// 查询参数
        /// </summary>
        public IpBlackQuery Query
        {
            get;
            set;
        }
    }
}
