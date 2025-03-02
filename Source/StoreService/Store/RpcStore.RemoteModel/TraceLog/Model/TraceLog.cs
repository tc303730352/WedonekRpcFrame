namespace RpcStore.RemoteModel.TraceLog.Model
{
    /// <summary>
    /// 链路日志
    /// </summary>
    public class TraceLog
    {
        /// <summary>
        /// 数据ID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 下级
        /// </summary>
        public TraceLog[] Children { get; set; }


        /// <summary>
        /// 事件指令
        /// </summary>
        public string Dictate { get; set; }
        /// <summary>
        /// 说明
        /// </summary>
        public string Show { get; set; }
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
        /// <summary>
        /// 发服务节点类型名
        /// </summary>
        public string SystemTypeName { get; set; }


        /// <summary>
        /// 接收的服务节点名
        /// </summary>
        public string RemoteServerName
        {
            get;
            set;
        }
        /// <summary>
        /// 机房
        /// </summary>
        public string Region { get; set; }
    }
}
