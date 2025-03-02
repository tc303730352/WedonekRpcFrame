using SqlSugar;
using WeDonekRpc.Model;

namespace RpcStore.Model.DB
{
    [SugarTable("RpcMerConfig")]
    public class RpcMerConfigModel
    {
        [SugarColumn(IsPrimaryKey = true)]
        public long Id
        {
            get;
            set;
        }
        public long RpcMerId
        {
            get;
            set;
        }
        public long SystemTypeId
        {
            get;
            set;
        }
        /// <summary>
        /// 是否隔离
        /// </summary>
        public bool IsRegionIsolate
        {
            get;
            set;
        }
        /// <summary>
        /// 隔离级别
        /// </summary>
        public bool IsolateLevel
        {
            get;
            set;
        }
        /// <summary>
        /// 负载均衡的方式
        /// </summary>
        public BalancedType BalancedType
        {
            get;
            set;
        }
    }
}
