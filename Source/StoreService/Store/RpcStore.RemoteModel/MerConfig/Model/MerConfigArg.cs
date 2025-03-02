using WeDonekRpc.Helper.Validate;
using WeDonekRpc.Model;

namespace RpcStore.RemoteModel.MerConfig.Model
{
    /// <summary>
    /// 集群配置
    /// </summary>
    public class MerConfigArg
    {
        /// <summary>
        /// 集群Id
        /// </summary>
        [NumValidate("rpc.store.mer.id.null", 1)]
        public long RpcMerId
        {
            get;
            set;
        }
        /// <summary>
        /// 节点类型
        /// </summary>
        [NumValidate("rpc.store.server.type.id.null", 1)]
        public long SystemTypeId
        {
            get;
            set;
        }
        /// <summary>
        /// 是否启用区域隔离
        /// </summary>
        public bool IsRegionIsolate
        {
            get;
            set;
        }
        /// <summary>
        /// 隔离级别 （true=完全隔离只能访问同机房的节点  false = 同机房的节点优先 只有同机房的节点都不可用时使用跨机房节点 ）
        /// </summary>
        public bool IsolateLevel
        {
            get;
            set;
        }
        /// <summary>
        /// 负载均衡的方式
        /// </summary>
        [EnumValidate("rpc.store.balancedType.error", typeof(BalancedType))]
        public BalancedType BalancedType
        {
            get;
            set;
        }
    }
}
