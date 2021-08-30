
using RpcClient;

using RpcModel;

namespace RpcModularModel.Accredit
{
        [IRemoteConfig("ApplyAccredit", "sys.sync", IsAllowRetry = false)]
        public class ApplyAccredit : RpcRemote<Model.ApplyAccreditRes>
        {

                /// <summary>
                /// 申请ID
                /// </summary>
                [RemoteLockAttr]
                public string ApplyId
                {
                        get;
                        set;
                }

                /// <summary>
                /// 申请角色类型
                /// </summary>
                [RemoteLockAttr]
                [TransmitColumn(TransmitType.ZoneIndex)]
                public string RoleType
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
                /// <summary>
                /// 授权用户状态
                /// </summary>
                public string State
                {
                        get;
                        set;
                }
        }
}
