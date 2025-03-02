using WeDonekRpc.Client;

using WeDonekRpc.Model;
using WeDonekRpc.ModularModel.Accredit.Model;

namespace WeDonekRpc.ModularModel.Accredit
{
    [IRemoteConfig("sys.sync", Transmit = "Accredit")]
    public class SetAccredit : RpcRemote<ApplyAccreditRes>
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
        

        public string State
        {
            get;
            set;
        }
        public int? Expire { get; set; }
    }
}
