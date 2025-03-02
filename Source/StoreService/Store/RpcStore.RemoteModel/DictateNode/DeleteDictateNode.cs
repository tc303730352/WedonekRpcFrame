using WeDonekRpc.Client;

namespace RpcStore.RemoteModel.DictateNode
{
    /// <summary>
    /// 删除广播指令路由节点
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class DeleteDictateNode:RpcRemote
    {
        public long Id
        {
            get;
            set;
        }
    }
}
