using WeDonekRpc.Client;

namespace RpcStore.RemoteModel.RunState
{
    /// <summary>
    /// 查询服务节点运行状态
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class QueryRunState : BasicPage<Model.RunState>
    {
    }
}
