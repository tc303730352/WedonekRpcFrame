using WeDonekRpc.Client;
using RpcStore.RemoteModel.ServerType.Model;

namespace RpcStore.RemoteModel.ServerType
{
    /// <summary>
    /// 查询服务类别
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class QueryServerType : BasicPage<ServerTypeDatum>
    {
        /// <summary>
        /// 查询参数
        /// </summary>
        public ServerTypeQuery Query
        {
            get;
            set;
        }
    }
}
