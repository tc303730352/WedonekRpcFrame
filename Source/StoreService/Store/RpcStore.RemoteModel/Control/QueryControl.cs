using WeDonekRpc.Client;
using RpcStore.RemoteModel.Control.Model;

namespace RpcStore.RemoteModel.Control
{
    /// <summary>
    /// 查询服务中心
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class QueryControl : BasicPage<RpcControlData>
    {
    }
}
