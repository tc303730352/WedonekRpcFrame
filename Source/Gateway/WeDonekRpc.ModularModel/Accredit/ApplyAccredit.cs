
using WeDonekRpc.Client;
using WeDonekRpc.Model;

namespace WeDonekRpc.ModularModel.Accredit
{
    [IRemoteConfig("ApplyAccredit", "sys.sync", IsAllowRetry = false, Transmit = "Accredit")]
    public class ApplyAccredit : RpcRemote<Model.ApplyAccreditRes>
    {
        [TransmitColumn(TransmitType.ZoneIndex)]
        public string AccreditId
        {
            get;
            set;
        }
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
        public string RoleType
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
        /// <summary>
        /// 有效时间（秒）
        /// </summary>
        public int? Expire { get; set; }
        /// <summary>
        /// 父级授权码
        /// </summary>
        public string ParentId { get; set; }
        /// <summary>
        /// 是否继承
        /// </summary>
        public bool IsInherit { get; set; }
    }
}
