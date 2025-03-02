using WeDonekRpc.Helper.Validate;

namespace RpcStore.RemoteModel.ReduceInRank.Model
{
    /// <summary>
    /// 降级配置
    /// </summary>
    public class ReduceInRankAdd : ReduceInRankDatum
    {
        /// <summary>
        /// 数据Id
        /// </summary>
        [NumValidate("rpc.store.mer.id.null", 1)]
        public long RpcMerId
        {
            get;
            set;
        }
        /// <summary>
        /// 服务Id
        /// </summary>
        [NumValidate("rpc.store.server.id.null", 1)]
        public long ServerId
        {
            get;
            set;
        }
    }
}
