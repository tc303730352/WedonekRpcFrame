using RpcStore.RemoteModel;

namespace RpcStore.Model.SysEventLog
{
    public class SystemEventLog
    {
        public long Id
        {
            get;
            set;
        }

        /// <summary>
        /// 集群ID
        /// </summary>
        public long RpcMerId
        {
            get;
            set;
        }
        /// <summary>
        /// 节点ID
        /// </summary>
        public long ServerId
        {
            get;
            set;
        }
        /// <summary>
        /// 节点类型ID
        /// </summary>
        public long SystemTypeId
        {
            get;
            set;
        }
        /// <summary>
        /// 所在区域ID
        /// </summary>
        public int RegionId
        {
            get;
            set;
        }
        /// <summary>
        /// 事件源
        /// </summary>
        public int EventSourceId
        {
            get;
            set;
        }
        /// <summary>
        /// 事件名
        /// </summary>
        public string EventName { get; set; }
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
