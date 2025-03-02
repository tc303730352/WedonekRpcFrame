using WeDonekRpc.Model;

namespace AutoTask.Gateway.Model
{
    public class TaskRpcBroadcast
    {
        /// <summary>
        /// 广播指令
        /// </summary>
        public string SysDictate { get; set; }

        /// <summary>
        /// 远程服务节点地址
        /// </summary>
        public string[] TypeVal
        {
            get;
            set;
        }
        /// <summary>
        /// 节点列表
        /// </summary>
        public long[] ServerId
        {
            get;
            set;
        }
        /// <summary>
        /// 是否限定唯一
        /// </summary>
        public bool IsOnly
        {
            get;
            set;
        }
        /// <summary>
        /// 广播方式
        /// </summary>
        public BroadcastType BroadcastType
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
