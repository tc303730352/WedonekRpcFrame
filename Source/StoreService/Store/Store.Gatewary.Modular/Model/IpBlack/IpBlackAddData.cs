using RpcStore.RemoteModel;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Validate;

namespace Store.Gatewary.Modular.Model.IpBlack
{
    public class IpBlackAddData
    {
        /// <summary>
        /// 集群Id
        /// </summary>
        [NumValidate("rpc.store.mer.id.error", 1)]
        public long RpcMerId
        {
            get;
            set;
        }
        /// <summary>
        /// 系统类别
        /// </summary>
        [NullValidate("rpc.store.server.type.null")]
        [LenValidate("rpc.store.server.type.len", 5, 50)]
        [FormatValidate("rpc.store.server.type.error", ValidateFormat.字母点)]
        public string SystemType
        {
            get;
            set;
        }
        /// <summary>
        /// Ip类型
        /// </summary>
        [EntrustValidate("_CheckIp")]
        [EnumValidate("rpc.store.format.ip.type.error", typeof(IpType))]
        public IpType IpType { get; set; }

        [FormatValidate("rpc.store.format.ip6.error", ValidateFormat.IP6)]
        public string Ip6 { get; set; }
        /// <summary>
        /// 起始IP
        /// </summary>
        [FormatValidate("rpc.store.ipback.begin.ip.error", ValidateFormat.IP)]
        public string Ip
        {
            get;
            set;
        }
        /// <summary>
        /// 截止IP
        /// </summary>
        [FormatValidate("rpc.store.ipback.end.ip.error", ValidateFormat.IP)]
        public string EndIp { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        private static void _CheckIp (IpBlackAddData add)
        {
            if (add.IpType == IpType.Ip4 && add.Ip.IsNull())
            {
                throw new ErrorException("rpc.store.ipback.ip.null");
            }
            else if (add.IpType == IpType.Ip6 && add.Ip6.IsNull())
            {
                throw new ErrorException("rpc.store.ipback.ip6.null");
            }
        }
    }
}
