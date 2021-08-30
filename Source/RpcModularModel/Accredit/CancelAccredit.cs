using RpcClient;

using RpcModel;

namespace RpcModularModel.Accredit
{
        [IRemoteConfig("sys.sync")]
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
