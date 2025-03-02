using WeDonekRpc.Helper;

namespace RpcStore.RemoteModel.SysLog.Model
{
    public class SystemLog
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
        /// 链路Id
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
           /// 系统类别
           /// </summary>
        public string SystemTypeName
        {
            get;
            set;
        }
        public long ServerId
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
        /// 日志组
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
        /// 日志等级
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
        /// 添加时间
        /// </summary>
        public DateTime AddTime
        {
            get;
            set;
        }
    }
}
