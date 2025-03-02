namespace RpcStore.RemoteModel.MerConfig.Model
{
    /// <summary>
    /// 服务集群配置
    /// </summary>
    public class RpcMerConfigDatum
    {
        /// <summary>
        /// 配置ID
        /// </summary>
        public long Id
        {
            get;
            set;
        }
        /// <summary>
        /// 设置的系统类型ID
        /// </summary>
        public long SystemTypeId
        {
            get;
            set;
        }
        /// <summary>
        /// 系统类别
        /// </summary>
        public string SystemType
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
        /// 隔离级别（true=完全隔离只能访问同机房的节点  false = 同机房的节点优先 只有同机房的节点都不可用时使用跨机房节点 ）
        /// </summary>
        public bool IsolateLevel
        {
            get;
            set;
        }
    }
}
