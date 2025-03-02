using WeDonekRpc.Helper;

namespace WeDonekRpc.Client.Config
{
    public class LogConfig
    {
        /// <summary>
        /// 是否上传日志
        /// </summary>
        public bool IsUpLog { get; set; } = true;

        /// <summary>
        /// 日志上传级别
        /// </summary>
        public LogGrade LogGradeLimit { get; set; } = LogGrade.ERROR;
        /// <summary>
        /// 排除的日志组
        /// </summary>
        public string[] ExcludeLogGroup { get; set; }
    }

}
