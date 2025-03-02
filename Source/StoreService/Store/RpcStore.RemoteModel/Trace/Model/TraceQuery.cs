namespace RpcStore.RemoteModel.Trace.Model
{
    /// <summary>
    /// 链路查询参数
    /// </summary>
    public class TraceQuery
    {
        /// <summary>
        /// 查询关键字
        /// </summary>
        public string QueryKey
        {
            get;
            set;
        }
        /// <summary>
        /// 集群ID
        /// </summary>
        public long? RpcMerId
        {
            get;
            set;
        }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? Begin
        {
            get;
            set;
        }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? End
        {
            get;
            set;
        }
        /// <summary>
        /// 服务节点类型
        /// </summary>
        public string SystemType { get; set; }

        /// <summary>
        /// 区域Id
        /// </summary>
        public int? RegionId { get; set; }
    }
}
