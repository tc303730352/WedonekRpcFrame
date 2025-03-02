using SqlSugar;

namespace RpcStore.Model.DB
{
    /// <summary>
    /// 服务节点降级配置
    /// </summary>
    [SugarTable("ReduceInRankConfig")]
    public class ReduceInRankConfigModel
    {
        /// <summary>
        /// 数据Id
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public long Id
        {
            get;
            set;
        }
        /// <summary>
        /// 数据Id
        /// </summary>
        public long RpcMerId
        {
            get;
            set;
        }
        /// <summary>
        /// 服务Id
        /// </summary>
        public long ServerId
        {
            get;
            set;
        }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnable { get; set; }
        /// <summary>
        /// 限制数
        /// </summary>
        public int LimitNum
        {
            get;
            set;
        }
        /// <summary>
        /// 熔断错误数
        /// </summary>
        public int FusingErrorNum
        {
            get;
            set;
        }

        /// <summary>
        /// 刷新统计数的时间(秒)
        /// </summary>
        public int RefreshTime { get; set; }
        /// <summary>
        /// 最短融断时长
        /// </summary>
        public int BeginDuration
        {
            get;
            set;
        }
        /// <summary>
        /// 最长熔断时长
        /// </summary>
        public int EndDuration
        {
            get;
            set;
        }

    }
}
