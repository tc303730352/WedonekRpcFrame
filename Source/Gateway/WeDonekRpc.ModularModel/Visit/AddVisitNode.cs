using WeDonekRpc.Client;
using WeDonekRpc.Model;
using WeDonekRpc.ModularModel.Visit.Model;

namespace WeDonekRpc.ModularModel.Visit
{
        [IRemoteConfig ("sys.sync", IsReply = false)]
        public class AddVisitNode : RpcRemote
        {
                public RpcVisit Visit
                {
                        get;
                        set;
                }
        }
}
