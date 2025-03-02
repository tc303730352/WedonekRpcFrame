using WeDonekRpc.Client;
using RpcStore.RemoteModel.SysEventLog.Model;

namespace RpcStore.RemoteModel.SysEventLog
{
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class GetSysEventLog : RpcRemote<SysEventLogData>
    {
        public long Id { get; set; }
    }
}
