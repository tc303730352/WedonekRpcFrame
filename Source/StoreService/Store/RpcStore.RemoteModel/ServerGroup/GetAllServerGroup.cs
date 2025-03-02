using WeDonekRpc.Client;
using RpcStore.RemoteModel.ServerGroup.Model;

namespace RpcStore.RemoteModel.ServerGroup
{
    /// <summary>
    /// 获取所有服务组别
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class GetAllServerGroup : RpcRemoteArray<ServerGroupItem>
    {
    }
}
