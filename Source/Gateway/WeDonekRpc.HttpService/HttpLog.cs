using System;
using System.Net;
using System.Text;

using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Log;
using WeDonekRpc.HttpService.Config;

namespace WeDonekRpc.HttpService
{
    internal class HttpLog
    {
        private static readonly string _DefGroup = "http";
        internal static void AddErrorLog (Uri uri, Exception e)
        {
            new ErrorLog(e, "HTTP请求错误!", _DefGroup)
                        {
                              { "Uri", uri}
                        }.Save();
        }
        internal static void AddLog (string title, Uri uri, string show)
        {
            new InfoLog(title, show, _DefGroup)
                        {
                              { "Uri", uri}
                        }.Save();
        }
        public static LogInfo CreateLog (System.Net.HttpListenerRequest request, LogConfig config)
        {
            LogInfo log = new LogInfo("Http请求日志!", config.LogGroup, config.LogGrade)
            {
                   { "Uri",request.Url}
            };
            if (config.IsAllHead)
            {
                log.Add("Head", request.Headers.ToString());
                return log;
            }
            else if (!config.RecordHead.IsNull())
            {
                config.RecordHead.ForEach(c =>
                {
                    log.Add(c, request.Headers[c]);
                });
            }
            if (config.IsAllCookie)
            {
                log.Add("Cookie", request.Headers["Cookie"]);
            }
            else if (!config.RecordCookie.IsNull())
            {
                StringBuilder str = new StringBuilder();
                config.RecordCookie.ForEach(c =>
                {
                    Cookie cookie = request.Cookies[c];
                    if (cookie != null)
                    {
                        _ = str.AppendFormat("{0}={1};", c, cookie.Value);
                    }
                });
                log.Add("Cookie", str.ToString());
            }
            return log;
        }
    }
}
