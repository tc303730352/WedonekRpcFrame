using WeDonekRpc.Model;

namespace RpcStore.RemoteModel.Tran.Model
{
    /// <summary>
    /// 事务查询参数
    /// </summary>
    public class TransactionQuery
    {
        public string TranName { get; set; }
        /// <summary>
        /// 服务集群Id
        /// </summary>
        public long? RpcMerId { get; set; }
        /// <summary>
        /// 服务节点Id
        /// </summary>
        public long? ServerId { get; set; }
        /// <summary>
        /// 服务节点类型Id
        /// </summary>
        public string SystemType { get; set; }
        /// <summary>
        /// 区域Id
        /// </summary>
        public int? RegionId { get; set; }

        /// <summary>
        /// 事务状态
        /// </summary>
        public TransactionStatus[] TranStatus { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? Begin { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? End { get; set; }
    }
}
