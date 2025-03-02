using WeDonekRpc.Helper.Validate;
namespace RpcStore.RemoteModel.ServerEventSwitch.Model
{
    public class EventSwitchSet
    {
        /// <summary>
        /// 服务节点ID
        /// </summary>
        [NumValidate("rpc.store.server.id.error", 0)]
        public long ServerId
        {
            get;
            set;
        }
        /// <summary>
        /// 事件级别
        /// </summary>
        [EnumValidate("rpc.store.event.level.error", typeof(SysEventLevel))]
        public SysEventLevel EventLevel
        {
            get;
            set;
        }

        /// <summary>
        /// 事件类型
        /// </summary>
        [EnumValidate("rpc.store.event.type.error", typeof(SysEventType))]
        public SysEventType EventType
        {
            get;
            set;
        }

        /// <summary>
        /// 事件提示模板
        /// </summary>
        [NullValidate("rpc.store.event.msg.template.null")]
        [LenValidate("rpc.store.event.msg.template.len", 1, 300)]
        public string MsgTemplate
        {
            get;
            set;
        }
        /// <summary>
        /// 事件配置项
        /// </summary>
        [NullValidate("rpc.store.event.config.null")]
        [LenValidate("rpc.store.event.config.len", 1, 1000)]
        public Dictionary<string, object> EventConfig
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
