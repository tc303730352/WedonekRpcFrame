using WeDonekRpc.Helper.Validate;
using WeDonekRpc.Model;

namespace RpcStore.RemoteModel.TransmitScheme.Model
{
    public class TransmitGenerate
    {
        [NumValidate("rpc.store.rpc.mer.id.error", 1)]
        public long RpcMerId
        {
            get;
            set;
        }
        [NumValidate("rpc.store.rpc.system.type.id.error", 1)]
        public long SystemTypeId
        {
            get;
            set;
        }
        [EnumValidate("rpc.store.rpc.transmit.type.error", typeof(TransmitType))]
        public TransmitType TransmitType
        {
            get;
            set;
        }
        /// <summary>
        /// 版本号
        /// </summary>
        [NumValidate("rpc.store.rpc.ver.num.error", 0)]
        public int VerNum { get; set; }
    }
}
