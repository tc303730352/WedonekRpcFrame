namespace RpcStore.RemoteModel.Trace.Model
{
    public class RpcTrace
    {
        /// <summary>
        /// 数据ID
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 链路Id
        /// </summary>
        public string TraceId { get; set; }
        /// <summary>
        /// 来源服务ID
        /// </summary>
        public long ServerId { get; set; }
        /// <summary>
        /// 事件指令
        /// </summary>
        public string Dictate { get; set; }
        /// <summary>
        /// 说明
        /// </summary>
        public string Show { get; set; }
        /// <summary>
        /// 来源说明
        /// </summary>
        public string ServerName { get; set; }
        /// <summary>
        /// 起始时间
        /// </summary>
        public DateTime BeginTime { get; set; }

        /// <summary>
        /// 耗时（毫秒*1000）
        /// </summary>
        public int Duration { get; set; }
        /// <summary>
        /// 服务节点类型名
        /// </summary>
        public string SystemTypeName { get; set; }
        /// <summary>
        /// 机房区域ID
        /// </summary>
        public int RegionId { get; set; }
        /// <summary>
        /// 集群ID
        /// </summary>
        public long RpcMerId { get; set; }
        /// <summary>
        /// 所属集群
        /// </summary>
        public string RpcMer { get; set; }
        /// <summary>
        /// 服务节点类型
        /// </summary>
        public string SystemType { get; set; }
        /// <summary>
        /// 所在区域
        /// </summary>
        public string Region { get; set; }
    }
}
