using System;

using WeDonekRpc.Helper;

namespace WeDonekRpc.ExtendModel.SysError
{
    /// <summary>
    /// 系统错误日志
    /// </summary>
    public class SysErrorLog
    {
        /// <summary>
        /// 错误标题
        /// </summary>
        public string LogTitle
        {
            get;
            set;
        }
        /// <summary>
        /// 内容
        /// </summary>
        public string LogShow
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
        /// 错误类型
        /// </summary>
        public LogType LogType
        {
            get;
            set;
        }
        /// <summary>
        /// 日志类目
        /// </summary>
        public string LogGroup
        {
            get;
            set;
        }
        /// <summary>
        /// 错误等级
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
        /// 日志属性列表
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
