using System;
using WeDonekRpc.HttpService.Interface;
using WeDonekRpc.Helper;

namespace WeDonekRpc.HttpService
{
    public static class LinqHelper
    {
        /// <summary>
        /// 获取真实的URL地址 如果被代理 请添加：X-Real-Url 头
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static Uri ToRealUrl (this IHttpRequest request)
        {
            string uri = request.Headers["X-Real-Url"];
            if (uri.IsNull())
            {
                return request.Url;
            }
            return new Uri(uri);
        }
        public static Uri GetUri (this IHttpRequest request, string path)
        {
            if (path[0] == '/')
            {
                return new Uri(string.Format("{0}://{1}{2}", request.Url.Scheme, request.Url.Authority, path));
            }
            return new Uri(string.Format("{0}://{1}/{2}", request.Url.Scheme, request.Url.Authority, path));
        }
        public static string UAgent (this IHttpRequest request)
        {
            return request.Headers["user-agent"];
        }

        public static string ToHost (this IHttpRequest request)
        {
            string host = request.Headers["Host"];
            if (host.IsNull())
            {
                return request.Url.Host;
            }
            return host;
        }
        public static void CheckCross (this ICrossConfig config, IHttpHandler handler)
        {
            Uri uri = handler.Request.UrlReferrer;
            if (uri == null || uri.Authority == handler.Request.ToRealUrl().Authority)
            {
                return;
            }
            else if (config.CheckUrlReferrer(uri))
            {
                string header = handler.Request.Headers["Access-Control-Request-Headers"];
                if (string.IsNullOrEmpty(header))
                {
                    header = config.AllowHead;
                }
                handler.Response.SetHead("Access-Control-Allow-Credentials", "true");
                handler.Response.SetHead("Access-Control-Allow-Headers", header);
                handler.Response.SetHead("Access-Control-Allow-Origin", string.Format("{1}://{0}", uri.Authority, uri.Scheme));
                handler.Response.SetHead("Access-Control-Max-Age", config.MaxAge);
                handler.Response.SetHead("Access-Control-Request-Method", config.Method);
                if (handler.Request.HttpMethod == "OPTIONS")
                {
                    handler.Response.SetHttpStatus(System.Net.HttpStatusCode.NoContent);
                    handler.Response.End();
                }
            }
            else if (!config.AllowCredentials)
            {
                handler.Response.SetHttpStatus(System.Net.HttpStatusCode.NonAuthoritativeInformation);
                handler.Response.End();
            }
        }
        public static bool CheckUrlReferrer (this ICrossConfig config, Uri referrer)
        {
            if (!config.AllowCredentials)
            {
                return false;
            }
            return config.AllowUrlReferrer.IsNull() || config.AllowUrlReferrer.IsExists(a => a == referrer.Authority || referrer.Authority.EndsWith(a));
        }
    }
}
