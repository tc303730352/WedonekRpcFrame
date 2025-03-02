using WeDonekRpc.Client;
using RpcStore.RemoteModel.Mer.Model;

namespace RpcStore.RemoteModel.Mer
{
    /// <summary>
    /// 获取集群列表
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class GetMerItems : RpcRemoteArray<BasicRpcMer>
    {
    }
}
