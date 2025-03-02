using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Validate;

namespace RpcStore.RemoteModel.IpBlack.Model
{
    public class IpBlackSet
    {

        /// <summary>
        /// Ip类型
        /// </summary>
        [EntrustValidate("_CheckIp")]
        [EnumValidate("rpc.store.format.ip.type.error", typeof(IpType))]
        public IpType IpType { get; set; }

        /// <summary>
        /// Ip
        /// </summary>
        [FormatValidate("rpc.store.format.ip6.error", ValidateFormat.IP6)]
        public string Ip6 { get; set; }

        /// <summary>
        /// 起始IP
        /// </summary>

        public long Ip
        {
            get;
            set;
        }
        /// <summary>
        /// 截止IP
        /// </summary>
        public long? EndIp { get; set; }
        /// <summary>
        /// 过期时间
        /// </summary>
        [TimeValidate("rpc.store.ipback.overtime.error", TimeFormat.秒, 0)]
        public long OverTime { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        private static void _CheckIp (IpBlackAdd add)
        {
            if (add.IpType == IpType.Ip4)
            {
                if (add.Ip < 0)
                {
                    throw new ErrorException("rpc.store.ipback.ip.error");
                }
                else if (add.EndIp.HasValue)
                {
                    if (add.EndIp.Value < 0 || add.EndIp.Value < add.Ip)
                    {
                        throw new ErrorException("rpc.store.ipback.endip.error");
                    }
                }
            }
        }
    }
}
