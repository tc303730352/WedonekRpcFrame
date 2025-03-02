using WeDonekRpc.Client;
using RpcStore.RemoteModel.SysEventLog.Model;

namespace RpcStore.RemoteModel.SysEventLog
{
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class QuerySysEventLog : BasicPage<SystemEventLogDto>
    {
        public SysEventLogQuery Query { get; set; }
    }
}
