using RpcStore.RemoteModel;
namespace RpcStore.Model.ServerEventSwitch
{
    public class ServerEventSwitchSet
    {
        /// <summary>
        /// 服务节点ID
        /// </summary>
        public long ServerId
        {
            get;
            set;
        }
        /// <summary>
        /// 事件Key
        /// </summary>
        public string EventKey { get; set; }
        /// <summary>
        /// 事件级别
        /// </summary>
        public SysEventLevel EventLevel
        {
            get;
            set;
        }

        /// <summary>
        /// 事件类型
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
        public string EventConfig
        {
            get;
            set;
        }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnable
        {
            get;
            set;
        }
    }
}
