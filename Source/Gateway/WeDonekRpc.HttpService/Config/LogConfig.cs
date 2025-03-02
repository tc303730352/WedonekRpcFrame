using System;
using WeDonekRpc.HttpService.Collect;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Config;

namespace WeDonekRpc.HttpService.Config
{
    public class LogConfig
    {
        private static readonly string[] _defHead = new string[] { "UserAgent", "UserHostAddress", "UrlReferrer", "HttpMethod", "ContentType" };
        public LogConfig ()
        {
            IConfigSection section = LocalConfig.Local.GetSection("gateway:http:log");
            section.AddRefreshEvent(this._Refresh);
        }
        private void _Refresh (IConfigSection section, string name)
        {
            this.IsEnable = section.GetValue("IsEnable", false);
            this.LogGrade = section.GetValue("LogGrade", LogGrade.DEBUG);
            this.LogGroup = section.GetValue("LogGroup", "http");
            this.RecordHead = section.GetValue("RecordHead", _defHead);
            this.RecordCookie = section.GetValue("RecordCookie", Array.Empty<string>());
            this.ResponseHead = section.GetValue("ResponseHead", Array.Empty<string>());
            this._Init();
        }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnable
        {
            get;
            private set;
        } = false;
        /// <summary>
        /// 日志级别
        /// </summary>
        public LogGrade LogGrade { get; private set; } = LogGrade.DEBUG;
        /// <summary>
        /// 日志组
        /// </summary>
        public string LogGroup { get; private set; } = "http";

        public bool IsAllHead { get; private set; }
        /// <summary>
        /// 需记录的请求头(*全部)
        /// </summary>
        public string[] RecordHead
        {
            get;
            private set;
        }

        public bool IsAllCookie { get; private set; }
        /// <summary>
        /// 需记录的请求Cookie(*全部)
        /// </summary>
        public string[] RecordCookie
        {
            get;
            private set;
        }
        /// <summary>
        /// 需记录的响应头(*全部)
        /// </summary>
        public string[] ResponseHead
        {
            get;
            private set;
        }
        private void _Init ()
        {
            if (!this.RecordHead.IsNull())
            {
                this.IsAllHead = this.RecordHead.IsExists("*");
            }
            else
            {
                this.IsAllHead = false;
            }
            if (!this.RecordCookie.IsNull())
            {
                this.IsAllCookie = this.RecordCookie.IsExists("*");
            }
            else
            {
                this.IsAllCookie = false;
            }
            ServiceCollect.InitHttpExecution(this);
        }
    }
}
