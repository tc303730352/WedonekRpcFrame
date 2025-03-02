using WeDonekRpc.Helper.Validate;
using WeDonekRpc.Model;

namespace RpcStore.RemoteModel.TransmitScheme.Model
{
    public class TransmitSchemeSet
    {
        /// <summary>
        /// 服务类型ID
        /// </summary>
        [NumValidate("rpc.store.server.type.error", 1)]
        public long SystemTypeId
        {
            get;
            set;
        }
        /// <summary>
        /// 方案名称
        /// </summary>
        [NullValidate("rpc.store.scheme.name.null")]
        [LenValidate("rpc.store.scheme.name.len", 6, 50)]
        [FormatValidate("rpc.store.scheme.name.error", ValidateFormat.数字字母点)]
        public string Scheme
        {
            get;
            set;
        }

        /// <summary>
        /// 负载均衡类型
        /// </summary>
        [EnumValidate("rpc.store.transmitType.error", typeof(TransmitType))]
        public TransmitType TransmitType
        {
            get;
            set;
        }
        /// <summary>
        /// 备注说明
        /// </summary>
        [LenValidate("rpc.store.scheme.show.len", 0, 50)]
        public string Show
        {
            get;
            set;
        }
        /// <summary>
        /// 应用版本号
        /// </summary>
        public int VerNum { get; set; }
    }
}
