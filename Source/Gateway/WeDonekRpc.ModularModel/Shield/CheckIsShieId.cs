using WeDonekRpc.Client;
using WeDonekRpc.Model;
using WeDonekRpc.ModularModel.Shield.Model;

namespace WeDonekRpc.ModularModel.Shield
{
    [IRemoteConfig("sys.extend")]
    public class CheckIsShieId : RpcRemote<ShieldDatum>
    {
        public string Path
        {
            get;
            set;
        }
        public ShieldType ShieldType
        {
            get;
            set;
        }
    }
}
