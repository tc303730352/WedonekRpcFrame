using WeDonekRpc.Client;

using WeDonekRpc.Model;

namespace WeDonekRpc.ModularModel.Accredit
{
    [IRemoteConfig("sys.sync", Transmit = "Accredit", IsProhibitTrace = true)]
    public class CheckAccredit : RpcRemote<int>
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
        public string CheckKey
        {
            get;
            set;
        }
        /// <summary>
        /// 是否刷新授权时间
        /// </summary>
        public bool IsRefresh { get; set; }
    }
}
