using WeDonekRpc.Helper.Validate;
using WeDonekRpc.Model;

namespace RpcStore.RemoteModel.ServerBind.Model
{
    public class BindGetParam
    {
        [NumValidate("rpc.store.mer.id.error", 1)]
        public long RpcMerId { get; set; }

        public int? RegionId { get; set; }
        /// <summary>
        /// 服务类型
        /// </summary>
        public RpcServerType? ServerType { get; set; }

        /// <summary>
        /// 是否是持有的
        /// </summary>
        public bool? IsHold { get; set; }

    }
}
