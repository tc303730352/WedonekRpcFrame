using WeDonekRpc.Client;
using RpcStore.RemoteModel.DictateNode.Model;

namespace RpcStore.RemoteModel.DictateNode
{
    /// <summary>
    /// 获取所有广播指令节点路由树形结构
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class GetDictateNodeTree : RpcRemoteArray<TreeDictateNode>
    {
    }
}
