using WeDonekRpc.ModularModel;

namespace RpcSync.Model
{
    public class ServiceEventDatum
    {
        /// <summary>
        /// 事件ID
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 系统事件ID
        /// </summary>
        public int SysEventId
        {
            get;
            set;
        }
        /// <summary>
        /// 事件名
        /// </summary>
        public string EventName
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
        /// 事件提示模板
        /// </summary>
        public string MsgTemplate
        {
            get;
            set;
        }
    }
}
