using WeDonekRpc.Helper.Validate;

namespace RpcStore.RemoteModel.ServerEventSwitch.Model
{
    public class EventSwitchAdd : EventSwitchSet
    {
        /// <summary>
        /// 集群ID
        /// </summary>
        [NullValidate("rpc.store.mer.Id.error")]
        public long RpcMerId { get; set; }


        /// <summary>
        /// 系统事件ID
        /// </summary>
        [NumValidate("rpc.store.event.id.error", 1, int.MaxValue)]
        public int SysEventId
        {
            get;
            set;
        }
    }
}
