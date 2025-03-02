using WeDonekRpc.Client;
using WeDonekRpc.Model;
using WeDonekRpc.ModularModel.IpBlack.Model;

namespace WeDonekRpc.ModularModel.IpBlack
{
    [IRemoteConfig("sys.extend")]
    public class SyncIpBlack : RpcRemote<IpBlackList>
    {
        public long LocalVer
        {
            get;
            set;
        }
    }
}
