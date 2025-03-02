using WeDonekRpc.Helper;

namespace RpcStore.RemoteModel.SysLog.Model
{
    /// <summary>
    /// 系统日志数据
    /// </summary>
    public class SystemLogData
    {
        /// <summary>
        /// 数据ID
        /// </summary>
        public long Id
        {
            get;
            set;
        }
        /// <summary>
        /// 集群ID
        /// </summary>
        public long RpcMerId
        {
            get;
            set;
        }
        /// <summary>
        /// 集群名
        /// </summary>
        public string RpcMerName
        {
            get;
            set;
        }
        /// <summary>
        /// 关联的链路ID
        /// </summary>
        public string TraceId
        {
            get;
            set;
        }

        /// <summary>
        /// 日志标题
        /// </summary>
        public string LogTitle
        {
            get;
            set;
        }
        /// <summary>
        /// 日志说明
        /// </summary>
        public string LogShow
        {
            get;
            set;
        }
        /// <summary>
        /// 系统类别
        /// </summary>
        public string SystemType
        {
            get;
            set;
        }
        /// <summary>
        /// 系统类别名
        /// </summary>
        public string SystemTypeName
        {
            get;
            set;
        }
        /// <summary>
        /// 服务节点ID
        /// </summary>
        public long ServerId
        {
            get;
            set;
        }
        /// <summary>
        /// 服务名
        /// </summary>
        public string ServerName
        {
            get;
            set;
        }
        /// <summary>
        /// 日志组别
        /// </summary>
        public string LogGroup
        {
            get;
            set;
        }
        /// <summary>
        /// 日志类型
        /// </summary>
        public LogType LogType
        {
            get;
            set;
        }
        /// <summary>
        /// 日志级别
        /// </summary>
        public LogGrade LogGrade
        {
            get;
            set;
        }
        /// <summary>
        /// 错误码
        /// </summary>
        public string ErrorCode
        {
            get;
            set;
        }
        /// <summary>
        /// 错误信息
        /// </summary>
        public ExceptionMsg Exception
        {
            get;
            set;
        }
        /// <summary>
        /// 错误属性
        /// </summary>
        public string AttrList
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
