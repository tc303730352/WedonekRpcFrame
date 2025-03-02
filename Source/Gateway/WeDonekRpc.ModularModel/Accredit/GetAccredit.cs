using WeDonekRpc.Client;

using WeDonekRpc.Model;
using WeDonekRpc.ModularModel.Accredit.Model;

namespace WeDonekRpc.ModularModel.Accredit
{
    [IRemoteConfig("sys.sync", Transmit = "Accredit", IsProhibitTrace = true)]
    public class GetAccredit : RpcRemote<AccreditDatum>
    {
        /// <summary>
        /// 授权ID
        /// </summary>
        [TransmitColumn(TransmitType.ZoneIndex)]
        public string AccreditId
        {
            get;
            set;
        }

    }
}
