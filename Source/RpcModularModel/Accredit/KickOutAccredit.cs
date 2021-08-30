using RpcClient;

using RpcModel;

namespace RpcModularModel.Accredit
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
