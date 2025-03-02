using WeDonekRpc.Helper.Validate;
using WeDonekRpc.Model;

namespace RpcStore.RemoteModel.TransmitScheme.Model
{
    public class TransmitSchemeQuery
    {
        /// <summary>
        /// 集群Id
        /// </summary>
        [NumValidate("rpc.store.mer.Id.error", 1)]
        public long RpcMerId
        {
            get;
            set;
        }
        /// <summary>
        /// 服务节点类型
        /// </summary>
        public long? SystemTypeId
        {
            get;
            set;
        }
        /// <summary>
        /// 方案名称
        /// </summary>
        public string Scheme
        {
            get;
            set;
        }

        /// <summary>
        /// 负载类型
        /// </summary>
        [EnumValidate("rpc.store.transmitType.error", typeof(TransmitType))]
        public TransmitType? TransmitType
        {
            get;
            set;
        }
        /// <summary>
        /// 版本号
        /// </summary>
        public int? VerNum
        {
            get;
            set;
        }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool? IsEnable
        {
            get;
            set;
        }
    }
}
