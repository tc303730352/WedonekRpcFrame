using System;

namespace WeDonekRpc.Helper.Log
{
    /// <summary>
    /// 错误日志
    /// </summary>
    public class ErrorLog : LogInfo
    {
        private const string _DefGroup = "Def";
        private const LogGrade _DefLogGrade = LogGrade.ERROR;

        /// <summary>
        /// 错误日志
        /// </summary>
        /// <param name="e">异常信息</param>
        public ErrorLog ( Exception e ) : base(e, _DefGroup, _DefLogGrade)
        {
        }
        /// <summary>
        /// 错误日志
        /// </summary>
        /// <param name="e">异常信息</param>
        /// <param name="title">标题</param>
        /// <param name="group">所属组别</param>
        public ErrorLog ( Exception e, string title, string group ) : base(e, group, _DefLogGrade)
        {
            this.LogTitle = title;
        }
        /// <summary>
        /// 错误日志
        /// </summary>
        /// <param name="e">异常信息</param>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <param name="group">所属组别</param>
        public ErrorLog ( Exception e, string title, string content, string group ) : base(e, group, _DefLogGrade)
        {
            this.LogContent = content;
            this.LogTitle = title;
        }
        /// <summary>
        /// 错误日志
        /// </summary>
        /// <param name="e">异常信息</param>
        /// <param name="title">标题</param>
        public ErrorLog ( Exception e, string title ) : base(e, _DefGroup, _DefLogGrade)
        {
            this.LogTitle = title;
        }
        /// <summary>
        /// 错误日志
        /// </summary>
        /// <param name="e">异常信息</param>
        public ErrorLog ( ErrorException e ) : base(e, _DefGroup)
        {
        }
        /// <summary>
        /// 错误日志
        /// </summary>
        /// <param name="e">异常信息</param>
        /// <param name="group">所属组别</param>
        public ErrorLog ( ErrorException e, string group ) : base(e, group)
        {
        }
        /// <summary>
        /// 错误日志
        /// </summary>
        /// <param name="e">异常信息</param>
        /// <param name="title">标题</param>
        /// <param name="group">所属组别</param>
        public ErrorLog ( ErrorException e, string title, string group ) : base(e, group)
        {
            this.LogTitle = title;
        }
        public ErrorLog ( ErrorException e, string title, string group, LogGrade grade ) : base(e, group, grade)
        {
            this.LogTitle = title;
        }
        /// <summary>
        /// 错误日志
        /// </summary>
        /// <param name="e">异常信息</param>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <param name="group">所属组别</param>
        public ErrorLog ( ErrorException e, string title, string content, string group ) : base(e, group)
        {
            this.LogContent = content;
            this.LogTitle = title;
        }
        public ErrorLog ( string error, string title, string group ) : base(error, _DefLogGrade, group)
        {
            this.LogTitle = title;
        }
        public ErrorLog ( string error, string title, string group, LogGrade grade ) : base(error, grade, group)
        {
            this.LogTitle = title;
        }
    }
}
