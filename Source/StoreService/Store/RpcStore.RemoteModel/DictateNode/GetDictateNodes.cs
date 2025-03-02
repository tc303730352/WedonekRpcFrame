using WeDonekRpc.Client;
using RpcStore.RemoteModel.DictateNode.Model;

namespace RpcStore.RemoteModel.DictateNode
{
    /// <summary>
    /// 获取广播路由节点
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class GetDictateNodes : RpcRemoteArray<DictateNodeData>
    {
        /// <summary>
        /// 父级ID
        /// </summary>
        public long? ParentId
        {
            get;
            set;
        }
    }
}
