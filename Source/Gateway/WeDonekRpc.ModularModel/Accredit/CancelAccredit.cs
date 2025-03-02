using WeDonekRpc.Client;

using WeDonekRpc.Model;

namespace WeDonekRpc.ModularModel.Accredit
{
    [IRemoteConfig("sys.sync", Transmit = "Accredit")]
    public class CancelAccredit : RpcRemote
    {
        [TransmitColumn(TransmitType.ZoneIndex)]
        public string AccreditId
        {
            get;
            set;
        }
        public string CheckKey { get; set; }
    }
}
