using RpcClient;

using RpcModel;

using RpcModularModel.Accredit.Model;

namespace RpcModularModel.Accredit
{
        [IRemoteConfig("sys.sync")]
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
