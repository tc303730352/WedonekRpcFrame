namespace RpcStore.RemoteModel.SysEventConfig.Model
{
    public class SystemEventConfig
    {
        /// <summary>
        /// 事件ID
        /// </summary>
        public int Id
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
        /// 模块
        /// </summary>
        public string Module
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
        /// <summary>
        /// 事件配置项
        /// </summary>
        public Dictionary<string, object> EventConfig
        {
            get;
            set;
        }
    }
}
