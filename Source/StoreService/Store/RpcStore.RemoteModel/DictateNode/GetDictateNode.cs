using WeDonekRpc.Client;
using RpcStore.RemoteModel.DictateNode.Model;

namespace RpcStore.RemoteModel.DictateNode
{
    /// <summary>
    /// 获取广播指令路由
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class GetDictateNode : RpcRemote<DictateNodeData>
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
