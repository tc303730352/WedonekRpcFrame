using WeDonekRpc.Client;
using RpcStore.RemoteModel.ServerType.Model;

namespace RpcStore.RemoteModel.ServerType
{
    /// <summary>
    /// 修改节点类型资料
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class SetServerType : RpcRemote
    {
        /// <summary>
        /// 数据ID
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 类别资料
        /// </summary>
        public ServerTypeSet Datum { get; set; }
    }
}
