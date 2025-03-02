namespace WeDonekRpc.Model
{
    public class BroadcastDatum
    {
        /// <summary>
        /// 消息关键字
        /// </summary>
        public string MsgKey
        {
            get;
            set;
        }
        /// <summary>
        /// 是否禁止链路
        /// </summary>
        public bool IsProhibitTrace
        {
            get;
            set;
        }
        /// <summary>
        /// 消息体
        /// </summary>
        public string MsgBody
        {
            get;
            set;
        }
        /// <summary>
        /// 限定服务区域Id
        /// </summary>
        public int? RegionId { get; set; }
        /// <summary>
        /// 集群Id
        /// </summary>
        public long? RpcMerId { get; set; }

        /// <summary>
        /// 消息配置
        /// </summary>
        public BasicRemoteConfig MsgConfig
        {
            get;
            set;
        }
    }
}
