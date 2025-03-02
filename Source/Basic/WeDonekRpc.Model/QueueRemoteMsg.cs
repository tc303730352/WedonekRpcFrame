namespace WeDonekRpc.Model
{
    public class QueueRemoteMsg
    {
        /// <summary>
        /// 队列消息
        /// </summary>
        public TcpRemoteMsg Msg
        {
            get;
            set;
        }
        /// <summary>
        /// 消息类型
        /// </summary>
        public string Type
        {
            get;
            set;
        }
        /// <summary>
        /// 是否已事务执行
        /// </summary>
        public long? TranId { get; set; }
        /// <summary>
        /// 是否排除来源
        /// </summary>
        public bool IsExclude
        {
            get;
            set;
        }
    }
}
