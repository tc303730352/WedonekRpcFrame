using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Log;
using WeDonekRpc.HttpService.Interface;
using WeDonekRpc.HttpService.Model;
using WeDonekRpc.HttpService.ResponseStream;

namespace WeDonekRpc.HttpService.Handler
{
    internal class HttpResponse : IHttpResponse
    {
        private readonly HttpListenerResponse _Response = null;
        private readonly IBasicHandler _Handler;


        public HttpResponse ( HttpListenerResponse response, IBasicHandler handler )
        {
            this._Response = response;
            this._Handler = handler;
        }
        #region Coolie操作
        public void AppendCookie ( Cookie cookie )
        {
            if ( string.IsNullOrEmpty(cookie.Path) )
            {
                cookie.Path = "/";
            }
            this._Response.AppendCookie(cookie);
        }
        public void RemoveCookie ( string name )
        {
            if ( this._Response.Cookies != null && this._Response.Cookies.Count > 0 )
            {
                foreach ( Cookie i in this._Response.Cookies )
                {
                    if ( i.Name == name )
                    {
                        i.Value = "";
                        i.Expires = DateTime.Now.AddSeconds(-10);
                    }
                }
            }
        }

        public void SetCookie ( Cookie cookie )
        {
            if ( string.IsNullOrEmpty(cookie.Path) )
            {
                cookie.Path = "/";
            }
            this._Response.SetCookie(cookie);
        }
        #endregion

        #region 设置缓存
        private static readonly Dictionary<HttpCacheType, string> _CacheTypeDic = new Dictionary<HttpCacheType, string> {
            { HttpCacheType.Public,"public" },
            { HttpCacheType.Private,"private" },
            { HttpCacheType.NoCache,"no-cache" },
        };
        public void SetCache ( HttpCacheSet cache )
        {
            if ( cache.CacheType == HttpCacheType.NoStore )
            {
                this._Response.AddHeader("Cache-Control", "no-store");
                return;
            }
            else if ( cache.MaxAge.HasValue || cache.SMaxAge.HasValue )
            {
                this._Response.AddHeader("Cache-Control", string.Format("{0},max-age={1}, s-maxage={2}{3}",
                    _CacheTypeDic[cache.CacheType],
                    cache.MaxAge.GetValueOrDefault(),
                    cache.SMaxAge.GetValueOrDefault(),
                    cache.MustRevalidate ? ",must-revalidate" : ";"));
            }
            else
            {
                this._Response.AddHeader("Cache-Control", _CacheTypeDic[cache.CacheType]);
            }
            if ( cache.Etag.IsNotNull() )
            {
                this._Response.AddHeader("Etag", cache.Etag);
            }
            else if ( cache.LastModified.HasValue )
            {
                this._Response.AddHeader("Last-Modified", cache.LastModified.Value.ToString("r"));
            }
        }

        #endregion

        /// <summary>
        /// 响应的文本
        /// </summary>
        public string ResponseTxt
        {
            get;
            private set;
        }
        public int StatusCode => this._Response.StatusCode;
        public bool IsEnd { get; private set; } = false;
        public string ContentType { get => this._Response.ContentType; set => this._Response.ContentType = value; }

        public void SetHttpStatus ( HttpStatusCode status )
        {
            this._Response.StatusCode = (int)status;
            this._Response.StatusDescription = status.ToString();
            this.IsEnd = true;
        }
        private bool _End ()
        {
            if ( this.IsEnd )
            {
                return false;
            }
            this.IsEnd = true;
            return true;
        }
        #region 302跳转
        public void Redirect ( Uri url )
        {
            this.Redirect(url.AbsoluteUri);
        }
        public void Redirect ( string url )
        {
            if ( !this._End() )
            {
                return;
            }
            try
            {
                this._Response.Redirect(url);
                this._Response.RedirectLocation = url;
                this._Response.StatusCode = 302;
            }
            catch ( Exception e )
            {
                new ErrorLog(e, "跳转错误!", "uri:" + url, "Http").Save();
            }
        }
        /// <summary>
        /// 写入文本
        /// </summary>
        /// <param name="text"></param>
        public void Write ( string text )
        {
            if ( !this._End() )
            {
                return;
            }
            this.ResponseTxt = text;
            using ( IResponseStream stream = new GzipBasicStream(this._Response, null) )
            {
                stream.WriteText(text, HttpService.Config.ResponseEncoding);
            }
        }
        public void Write ( string text, Encoding encoding )
        {
            if ( !this._End() )
            {
                return;
            }
            this.ResponseTxt = text;
            using ( IResponseStream stream = new GzipBasicStream(this._Response, null) )
            {
                stream.WriteText(text, encoding);
            }
        }
        /// <summary>
        /// 写入文件
        /// </summary>
        /// <param name="file"></param>
        public void WriteFile ( FileInfo file )
        {
            if ( !this._End() )
            {
                return;
            }
            using ( IResponseStream stream = new GzipBasicStream(this._Response, this._Handler.Request.Headers["Range"]) )
            {
                stream.WriteFile(file);
            }
        }
        /// <summary>
        /// 写入流
        /// </summary>
        /// <param name="stream"></param>
        public void WriteStream ( Stream stream, string extension )
        {
            if ( !this._End() )
            {
                return;
            }
            using ( IResponseStream response = new GzipBasicStream(this._Response, this._Handler.Request.Headers["Range"]) )
            {
                response.WriteStream(stream, extension);
            }
        }


        #endregion
        public void SetHead ( string name, string value )
        {
            this._Response.Headers.Set(name, value);
        }

        public void Dispose ()
        {
            this._Response.Close();
        }

        public void End ()
        {
            this.IsEnd = true;
            this._Handler.End();
        }
    }
}
