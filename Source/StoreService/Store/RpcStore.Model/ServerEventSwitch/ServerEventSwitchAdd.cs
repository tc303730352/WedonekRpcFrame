namespace RpcStore.Model.ServerEventSwitch
{
    public class ServerEventSwitchAdd : ServerEventSwitchSet
    {
        /// <summary>
        /// 集群ID
        /// </summary>
        public long RpcMerId { get; set; }

        /// <summary>
        /// 系统事件ID
        /// </summary>
        public int SysEventId
        {
            get;
            set;
        }
        /// <summary>
        /// 模块名
        /// </summary>
        public string Module { get; set; }
    }
}
