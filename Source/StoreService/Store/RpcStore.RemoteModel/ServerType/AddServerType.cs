using WeDonekRpc.Client;
using RpcStore.RemoteModel.ServerType.Model;

namespace RpcStore.RemoteModel.ServerType
{
    /// <summary>
    /// 添加服务类别
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class AddServerType : RpcRemote<long>
    {
        /// <summary>
        /// 类别资料
        /// </summary>
        public ServerTypeAdd Datum
        {
            get;
            set;
        }
    }
}
