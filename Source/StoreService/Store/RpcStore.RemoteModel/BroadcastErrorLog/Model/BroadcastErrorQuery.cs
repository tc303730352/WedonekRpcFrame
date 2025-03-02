namespace RpcStore.RemoteModel.BroadcastErrorLog.Model
{
    public class BroadcastErrorQuery
    {
        /// <summary>
        /// 消息Key
        /// </summary>
        public string MsgKey { get; set; }

        /// <summary>
        /// 起始时间
        /// </summary>
        public DateTime? Begin { get; set; }
        /// <summary>
        /// 截止时间
        /// </summary>
        public DateTime? End { get; set; }
    }
}
