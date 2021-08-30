using RpcClient;

using RpcModel;

namespace RpcModularModel.Accredit
{
        [IRemoteConfig("sys.sync")]
        public class SetUserState : RpcRemote<SetUserStateRes>
        {
                [TransmitColumn(TransmitType.ZoneIndex)]
                public string AccreditId
                {
                        get;
                        set;
                }
                public string UserState
                {
                        get;
                        set;
                }
                public long VerNum { get; set; }
                /// <summary>
                /// 是否强制覆盖
                /// </summary>
                public bool IsCover { get; set; }
        }
}
