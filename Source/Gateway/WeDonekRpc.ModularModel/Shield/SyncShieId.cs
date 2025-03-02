using WeDonekRpc.Client;
using WeDonekRpc.Model;

namespace WeDonekRpc.ModularModel.Shield
{
    [IRemoteConfig("sys.extend")]
    public class SyncShieId : RpcRemote
    {
        public string Path
        {
            get;
            set;
        }
        public long RpcMerId
        {
            get;
            set;
        }
        public long ServerId
        {
            get;
            set;
        }
        public string SystemType
        {
            get;
            set;
        }
        public int VerNum
        {
            get;
            set;
        }
        public ShieldType ShieldType { get; set; }
    }
}
