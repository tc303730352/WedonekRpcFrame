using WeDonekRpc.Model;

namespace RpcStore.RemoteModel.BroadcastErrorLog.Model
{
    public class BroadcastLogDatum
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
        /// 消息Key
        /// </summary>
        public string MsgKey
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
        /// 来源服务节点ID
        /// </summary>
        public long SourceId { get; set; }
        /// <summary>
        /// 系统类型
        /// </summary>
        public string SysTypeVal
        {
            get;
            set;
        }


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
        /// <summary>
        /// 错误码
        /// </summary>
        public string Error
        {
            get;
            set;
        }
        /// <summary>
        /// 错误文本
        /// </summary>
        public string ErrorText { get; set; }
        /// <summary>
        /// 系统类型
        /// </summary>
        public string SystemTypeName
        {
            get;
            set;
        }
        /// <summary>
        /// 服务节点名
        /// </summary>
        public string ServerName
        {
            get;
            set;
        }
        /// <summary>
        /// 来源服务名
        /// </summary>
        public string SourceServer
        {
            get;
            set;
        }
        /// <summary>
        /// 集群Id
        /// </summary>
        public string RpcMer
        {
            get;
            set;
        }
    }
}
