using WeDonekRpc.Helper.Validate;
namespace RpcStore.RemoteModel.ServerEventSwitch.Model
{
    public class EventSwitchQuery
    {
        /// <summary>
        /// 集群ID
        /// </summary>
        [NumValidate("rpc.store.mer.id.error", 1)]
        public long RpcMerId { get; set; }

        /// <summary>
        /// 服务节点ID
        /// </summary>
        public long? ServerId { get; set; }
        /// <summary>
        /// 系统事件ID
        /// </summary>
        public int? SysEventId { get; set; }

        /// <summary>
        /// 事件级别
        /// </summary>
        public SysEventLevel? EventLevel
        {
            get;
            set;
        }
        /// <summary>
        /// 系统事件类别
        /// </summary>
        public SysEventType? EventType
        {
            get;
            set;
        }
    }
}
