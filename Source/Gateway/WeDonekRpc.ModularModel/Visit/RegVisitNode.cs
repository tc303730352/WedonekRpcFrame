using WeDonekRpc.Client;
using WeDonekRpc.Model;
using WeDonekRpc.ModularModel.Visit.Model;

namespace WeDonekRpc.ModularModel.Visit
{
        [IRemoteConfig ("sys.sync", IsReply = false)]
        public class RegVisitNode : RpcRemote
        {
                public RpcVisit[] Visits
                {
                        get;
                        set;
                }
        }
}
