using RpcClient;

using RpcModel;

using RpcModularModel.Accredit.Model;

namespace RpcModularModel.Accredit
{
        [IRemoteConfig("sys.sync")]
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
                /// <summary>
                /// 授权角色列表
                /// </summary>
                public string[] AccreditRole
                {
                        get;
                        set;
                }

                public string State
                {
                        get;
                        set;
                }

        }
}
