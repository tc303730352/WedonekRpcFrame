using WeDonekRpc.Client;
using RpcStore.RemoteModel.ServerGroup.Model;

namespace RpcStore.RemoteModel.ServerGroup
{
    /// <summary>
    /// 获取服务组信息
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class GetServerGroup : RpcRemote<ServerGroupDatum>
    {
        /// <summary>
        /// 数据ID
        /// </summary>
        public long Id
        {
            get;
            set;
        }
    }
}
