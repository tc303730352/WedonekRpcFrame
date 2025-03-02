namespace AutoTask.Gateway.Model
{
    public class TaskRpcParam
    {
        public string SysDictate
        {
            get;
            set;
        }
      
        /// <summary>
        /// 服务器类型
        /// </summary>
        public string SystemType
        {
            get;
            set;
        }
        /// <summary>
        /// 服务节点Id
        /// </summary>
        public long ServerId
        {
            get;
            set;
        }

        /// <summary>
        /// 消息体
        /// </summary>
        public Dictionary<string, object> MsgBody { get; set; }


        /// <summary>
        /// 目的地
        /// </summary>
        public string ToAddress
        {
            get;
            set;
        }
    }
}
