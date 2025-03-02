using SqlSugar;
using WeDonekRpc.Model;

namespace RpcCentral.Model.DB
{
    [SugarTable("RpcMerConfig")]
    public class RpcMerConfig
    {
        [SugarColumn(IsPrimaryKey = true)]
        public long Id { get; set; }

        public long RpcMerId { get; set; }

        public long SystemTypeId { get; set; }
        /// <summary>
        /// 是否隔离
        /// </summary>
        public bool IsRegionIsolate
        {
            get;
            set;
        }
        /// <summary>
        /// 隔离级别（true=完全隔离只能访问同机房的节点  false = 同机房的节点优先 只有同机房的节点都不可用时使用跨机房节点 ）
        /// </summary>
        public bool IsolateLevel
        {
            get;
            set;
        }
        /// <summary>
        /// 负载均衡类型
        /// </summary>
        public BalancedType BalancedType
        {
            get;
            set;
        }
    }
}
