namespace RpcStore.RemoteModel.ServerEventSwitch.Model
{
    /// <summary>
    /// 系统事件
    /// </summary>
    public class EventSwitch
    {
        public long Id { get; set; }
        /// <summary>
        /// 服务节点ID
        /// </summary>
        public long ServerId { get; set; }

        /// <summary>
        /// 服务名
        /// </summary>
        public string ServerName { get; set; }


        public int SysEventId { get; set; }

        /// <summary>
        /// 事件名
        /// </summary>
        public string SysEventName { get; set; }
        /// <summary>
        /// 模块名
        /// </summary>
        public string Module { get; set; }
        /// <summary>
        /// 事件级别
        /// </summary>
        public SysEventLevel EventLevel
        {
            get;
            set;
        }
        /// <summary>
        /// 系统事件类别
        /// </summary>
        public SysEventType EventType
        {
            get;
            set;
        }
        /// <summary>
        /// 消息摸版
        /// </summary>
        public string MsgTemplate { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnable { get; set; }
    }
}
