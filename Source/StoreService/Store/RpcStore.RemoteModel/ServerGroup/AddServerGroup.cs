using WeDonekRpc.Client;
using RpcStore.RemoteModel.ServerGroup.Model;

namespace RpcStore.RemoteModel.ServerGroup
{
    /// <summary>
    /// 添加服务组
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class AddServerGroup : RpcRemote<long>
    {
        /// <summary>
        /// 服务组资料
        /// </summary>
        public ServerGroupAdd Group
        {
            get;
            set;
        }
    }
}
