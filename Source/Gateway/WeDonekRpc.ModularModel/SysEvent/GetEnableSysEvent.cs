using WeDonekRpc.Client;
using WeDonekRpc.Model;
using WeDonekRpc.ModularModel.SysEvent.Model;

namespace WeDonekRpc.ModularModel.SysEvent
{
    [IRemoteConfig("sys.sync")]
    public class GetEnableSysEvent : RpcRemoteArray<ServiceSysEvent>
    {
        public string Module { get; set; }
    }
}
