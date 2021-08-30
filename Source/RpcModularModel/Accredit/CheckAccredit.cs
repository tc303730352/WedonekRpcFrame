using RpcClient;

using RpcModel;

namespace RpcModularModel.Accredit
{
        [IRemoteConfig("sys.sync")]
        public class CheckAccredit : RpcRemote
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
