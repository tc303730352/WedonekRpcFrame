namespace RpcStore.RemoteModel.TraceLog.Model
{
    /// <summary>
    /// 链路日志
    /// </summary>
    public class TraceLogDatum
    {
        /// <summary>
        /// 数据ID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 链路ID
        /// </summary>
        public string TraceId { get; set; }

        /// <summary>
        /// SpanID
        /// </summary>
        public long SpanId { get; set; }

        /// <summary>
        /// 父级SpanId
        /// </summary>
        public long ParentId { get; set; }
        /// <summary>
        /// 发起服务节点名
        /// </summary>
        public string ServerName { get; set; }

        /// <summary>
        /// 事件指令
        /// </summary>
        public string Dictate { get; set; }
        /// <summary>
        /// 发起服务节点ID
        /// </summary>
        public long ServerId { get; set; }

        /// <summary>
        /// 发服务节点类型
        /// </summary>
        public string SystemType { get; set; }

        /// <summary>
        /// 连接的远端服务节点ID
        /// </summary>
        public long RemoteId { get; set; }

        /// <summary>
        /// 机房ID
        /// </summary>
        public int RegionId { get; set; }

        /// <summary>
        /// 返回结果
        /// </summary>
        public string ReturnRes { get; set; }

        /// <summary>
        /// 发送的参数
        /// </summary>
        public Dictionary<string, string> Args { get; set; }
        /// <summary>
        /// 发送的消息类型
        /// </summary>
        public string MsgType { get; set; }

        /// <summary>
        /// 发送方向（发送还是回复）
        /// </summary>
        public StageType StageType { get; set; }
        /// <summary>
        /// 发送开始时间
        /// </summary>
        public DateTime BeginTime { get; set; }
        /// <summary>
        /// 耗时(毫秒*1000)
        /// </summary>
        public int Duration { get; set; }
    }
}
