using System;
using System.IO;
using System.Net;
using System.Text;

using HttpService.Helper;
using HttpService.Interface;

using RpcHelper;

namespace HttpService.Handler
{
        internal class HttpResponse : IHttpResponse
        {
                private bool _IsEnd = false;
                private readonly HttpListenerResponse _Response = null;
                private readonly IBasicHandler _Handler;

                public HttpResponse(HttpListenerResponse response, IBasicHandler handler)
                {
                        this._Response = response;
                        this._Handler = handler;
                }
                #region Coolie操作
                public void AppendCookie(Cookie cookie)
                {
                        if (string.IsNullOrEmpty(cookie.Path))
                        {
                                cookie.Path = "/";
                        }
                        this._Response.AppendCookie(cookie);
                }
                public void RemoveCookie(string name)
                {
                        if (this._Response.Cookies != null && this._Response.Cookies.Count > 0)
                        {
                                foreach (Cookie i in this._Response.Cookies)
                                {
                                        if (i.Name == name)
                                        {
                                                i.Value = "";
                                                i.Expires = DateTime.Now.AddSeconds(-10);
                                        }
                                }
                        }
                }

                public void SetCookie(Cookie cookie)
                {
                        if (string.IsNullOrEmpty(cookie.Path))
                        {
                                cookie.Path = "/";
                        }
                        this._Response.SetCookie(cookie);
                }
                #endregion

                #region 设置缓存
                public void SetCache(string etag)
                {
                        //this._Response.AddHeader("Cache-Control", string.Format("max-age={0}, must-revalidate;", cacheTime));
                        this._Response.AddHeader("Etag", etag);
                }
                public void SetCache(int cacheTime)
                {
                        this._Response.AddHeader("Cache-Control", string.Format("max-age={0}, must-revalidate;", cacheTime));
                }
                public void SetCacheType(string cacheType)
                {
                        this._Response.AddHeader("cache-control", cacheType);
                }
                public void SetCache(DateTime toUpdateTime, int cacheTime)
                {
                        this._Response.AddHeader("Cache-Control", string.Format("max-age={0}, must-revalidate;", cacheTime));
                        this._Response.AddHeader("Last-Modified", toUpdateTime.ToString("r"));
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
                public bool IsEnd => this._IsEnd;
                public string ContentType { get => this._Response.ContentType; set => this._Response.ContentType = value; }

                public void SetHttpStatus(HttpStatusCode status)
                {
                        this._Response.StatusCode = (int)status;
                        this._Response.StatusDescription = status.ToString();
                }
                private bool _End()
                {
                        if (this._IsEnd)
                        {
                                return false;
                        }
                        this._IsEnd = true;
                        return true;
                }
                #region 302跳转
                public void Redirect(Uri url)
                {
                        this.Redirect(url.AbsoluteUri);
                }
                public void Redirect(string url)
                {
                        if (!this._End())
                        {
                                return;
                        }
                        try
                        {
                                this._Response.Redirect(url);
                                this._Response.RedirectLocation = url;
                                this._Response.StatusCode = 302;
                        }
                        catch (Exception e)
                        {
                                new ErrorLog(e, "跳转错误!", "uri:" + url, "Http").Save();
                        }
                }
                /// <summary>
                /// 写入文本
                /// </summary>
                /// <param name="text"></param>
                public void Write(string text)
                {
                        if (!this._End())
                        {
                                return;
                        }
                        this._Response.ContentEncoding = Config.ServerConfig.ResponseEncoding;
                        this.ResponseTxt = text;
                        byte[] myByte = Encoding.UTF8.GetBytes(text);
                        this._Response.ContentLength64 = myByte.Length;
                        using (Stream stream = this._Response.OutputStream)
                        {
                                stream.Write(myByte, 0, myByte.Length);
                                stream.Flush();
                        }
                }

                /// <summary>
                /// 写入文件
                /// </summary>
                /// <param name="file"></param>
                public void WriteFile(FileInfo file)
                {
                        if (!this._End())
                        {
                                return;
                        }
                        FileWriteHelper.WriteFile(this._Response, file, this._Handler.Request.Headers["Range"]);
                }
                /// <summary>
                /// 写入流
                /// </summary>
                /// <param name="stream"></param>
                public void WriteStream(Stream stream)
                {
                        if (!this._End())
                        {
                                return;
                        }
                        FileWriteHelper.WriteStream(this._Response, stream, this._Response.ContentType, this._Handler.Request.Headers["Range"]);
                }

                #endregion
                public void SetHead(string name, string value)
                {
                        this._Response.Headers.Set(name, value);
                }

                public void Dispose()
                {
                        this._Response.Close();
                }

                public void End()
                {
                        this._IsEnd = true;
                        this._Handler.End();
                }
        }
}
