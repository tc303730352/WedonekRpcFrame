using WeDonekRpc.Client;

using WeDonekRpc.Model;

namespace WeDonekRpc.ModularModel.Accredit
{
        [IRemoteConfig("KickOutAccredit", "sys.sync")]
        public class KickOutAccredit : RpcRemote
        {
                public string CheckKey
                {
                        get;
                        set;
                }
        }
}
