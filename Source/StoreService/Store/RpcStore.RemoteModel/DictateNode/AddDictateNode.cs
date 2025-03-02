using WeDonekRpc.Client;
using RpcStore.RemoteModel.DictateNode.Model;

namespace RpcStore.RemoteModel.DictateNode
{
    /// <summary>
    /// 添加广播指令路由节点
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class AddDictateNode : RpcRemote<long>
    {
        /// <summary>
        /// 路由节点信息
        /// </summary>
        public DictateNodeAdd Datum
        {
            get;
            set;
        }
    }
}
