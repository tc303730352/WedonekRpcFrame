using WeDonekRpc.Client;

using WeDonekRpc.Model;

namespace WeDonekRpc.ModularModel.Accredit
{
    [IRemoteConfig("sys.sync", Transmit = "Accredit")]
    public class SetUserState : RpcRemote<SetUserStateRes>
    {
        [TransmitColumn(TransmitType.ZoneIndex)]
        public string AccreditId
        {
            get;
            set;
        }

        public long VerNum { get; set; }


        /// <summary>
        /// 用户状态
        /// </summary>
        public string UserState { get; set; }
    }
}
