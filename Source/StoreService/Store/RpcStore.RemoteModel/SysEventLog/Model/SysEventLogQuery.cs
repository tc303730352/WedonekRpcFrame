using WeDonekRpc.Helper.Validate;
namespace RpcStore.RemoteModel.SysEventLog.Model
{
    public class SysEventLogQuery
    {
        /// <summary>
        /// 集群ID
        /// </summary>
        [NumValidate("rpc.store.mer.Id.error", 1)]
        public long RpcMerId
        {
            get;
            set;
        }

        /// <summary>
        /// 节点类型ID
        /// </summary>
        public long? SystemTypeId
        {
            get;
            set;
        }
        /// <summary>
        /// 所在区域ID
        /// </summary>
        public int? RegionId
        {
            get;
            set;
        }

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
        /// <summary>
        /// 来源节点
        /// </summary>
        public long? ServerId { get; set; }
    }
}
