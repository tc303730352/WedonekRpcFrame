using WeDonekRpc.Client;
using WeDonekRpc.Model;
using WeDonekRpc.ModularModel.Visit.Model;

namespace WeDonekRpc.ModularModel.Visit
{
    [IRemoteConfig("sys.sync", IsReply = false, IsProhibitTrace = true)]
    public class AddVisitLog : RpcRemote
    {
        public RpcDictateVisit[] Logs
        {
            get;
            set;
        }
    }
}
