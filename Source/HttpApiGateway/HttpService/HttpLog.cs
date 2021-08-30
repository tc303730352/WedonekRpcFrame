using System;

using HttpService.Interface;

using RpcHelper;

namespace HttpService
{
        internal class HttpLog
        {
                private static readonly string _DefGroup = "http";
                internal static void AddErrorLog(IHttpServer server, Exception e)
                {
                        new ErrorLog(e, "HTTP请求错误!", _DefGroup)
                        {
                              { "Uri",server.Uri}
                        }.Save();
                }

                public static LogInfo CreateLog(System.Net.HttpListenerRequest request)
                {
                        return new DebugLog("Http请求日志!", _DefGroup)
                        {
                                { "Uri",request.Url},
                                { "UserAgent",request.UserAgent},
                                { "UserAddress",request.UserHostAddress},
                                { "UrlReferrer",request.UrlReferrer},
                                { "HttpMethod",request.HttpMethod},
                                { "ContentType",request.ContentType},
                                { "ContentLength",request.ContentLength64},
                                { "Head",request.Headers.ToString()},
                                { "Cookie",request.Cookies.ToString()}
                        };
                }
        }
}
