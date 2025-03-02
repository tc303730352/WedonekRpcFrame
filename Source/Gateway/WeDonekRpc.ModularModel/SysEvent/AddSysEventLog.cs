using WeDonekRpc.Client;
using WeDonekRpc.Model;
using WeDonekRpc.ModularModel.SysEvent.Model;

namespace WeDonekRpc.ModularModel.SysEvent
{
    [IRemoteConfig("sys.sync", IsReply = false)]
    public class AddSysEventLog : RpcRemote
    {
        public SysEventLog[] Logs
        {
            get;
            set;
        }
    }
}
