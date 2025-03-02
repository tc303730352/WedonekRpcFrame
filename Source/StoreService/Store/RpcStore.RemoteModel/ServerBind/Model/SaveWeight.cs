using WeDonekRpc.Helper.Validate;

namespace RpcStore.RemoteModel.ServerBind.Model
{
    public class SaveWeight
    {
        /// <summary>
        /// 服务集群ID
        /// </summary>
        [NumValidate("rpc.store.mer.Id.error", 1)]
        public long RpcMerId { get; set; }
        /// <summary>
        /// 服务类型
        /// </summary>
        [NumValidate("rpc.store.server.type.error", 1)]
        public long SystemType { get; set; }
        /// <summary>
        /// 服务权重
        /// </summary>
        [NullValidate("rpc.store.server.weight.null")]
        public Dictionary<long, int> Weight
        {
            get;
            set;
        }
    }
}
