using WeDonekRpc.Client;
using RpcStore.RemoteModel.SysEventConfig.Model;

namespace RpcStore.RemoteModel.SysEventConfig
{
    /// <summary>
    /// 获取系统事件项
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class GetSysEventItems : RpcRemoteArray<SystemEventItem>
    {
    }
}
