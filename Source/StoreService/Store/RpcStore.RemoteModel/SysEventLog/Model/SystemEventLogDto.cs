namespace RpcStore.RemoteModel.SysEventLog.Model
{
    public class SystemEventLogDto
    {
        /// <summary>
        /// 事件日志ID
        /// </summary>
        public long Id
        {
            get;
            set;
        }
        /// <summary>
        /// 事件名
        /// </summary>
        public string EventName { get; set; }

        /// <summary>
        /// 节点ID
        /// </summary>
        public long ServerId
        {
            get;
            set;
        }
        /// <summary>
        /// 服务名
        /// </summary>
        public string ServerName { get; set; }


        /// <summary>
        /// 节点类型
        /// </summary>
        public string SystemType { get; set; }


        /// <summary>
        /// 所在区域
        /// </summary>
        public string Region
        {
            get;
            set;
        }

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
        /// 事件的说明
        /// </summary>
        public string EventShow
        {
            get;
            set;
        }
        /// <summary>
        /// 事件时间
        /// </summary>
        public DateTime AddTime
        {
            get;
            set;
        }
    }
}
