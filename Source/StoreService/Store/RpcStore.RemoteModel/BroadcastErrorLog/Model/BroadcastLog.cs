using WeDonekRpc.Model;

namespace RpcStore.RemoteModel.BroadcastErrorLog.Model
{
    /// <summary>
    /// 广播错误日志
    /// </summary>
    public class BroadcastLog
    {
        /// <summary>
        /// 日志Id
        /// </summary>
        public long Id
        {
            get;
            set;
        }
        /// <summary>
        /// 错误码
        /// </summary>
        public string Error
        {
            get;
            set;
        }
        /// <summary>
        /// 消息Key
        /// </summary>
        public string MsgKey
        {
            get;
            set;
        }
        /// <summary>
        /// 消息体
        /// </summary>
        public string MsgBody { get; set; }
        /// <summary>
        /// 服务节点Id
        /// </summary>
        public long ServerId
        {
            get;
            set;
        }
        /// <summary>
        /// 系统类型
        /// </summary>
        public string SysTypeVal
        {
            get;
            set;
        }

        /// <summary>
        /// 消息源
        /// </summary>
        public MsgSourceDto MsgSource { get; set; }

        /// <summary>
        /// 广播方式
        /// </summary>
        public BroadcastType BroadcastType { get; set; }
        /// <summary>
        /// 路由健
        /// </summary>
        public string RouteKey { get; set; }
        /// <summary>
        /// 集群Id
        /// </summary>
        public long RpcMerId
        {
            get;
            set;
        }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddTime
        {
            get;
            set;
        }
    }
}
