using System.Net;
using System.Text.RegularExpressions;

using HttpService.Interface;

namespace HttpService.Handler
{
        public class BasicHandler : IBasicHandler
        {
                private IHttpRequest _Request = null;
                private IHttpResponse _Response = null;

                /// <summary>
                /// 请求对象
                /// </summary>
                public IHttpRequest Request => this._Request;


                /// <summary>
                /// 响应对象
                /// </summary>
                public IHttpResponse Response => this._Response;
                public string SortNum { get; }

                private readonly bool _IsComplate;

                private readonly bool _IsRegex = false;

                private readonly Regex _Regex = null;

                /// <summary>
                /// 请求路径
                /// </summary>
                public string RequestPath
                {
                        get;
                }

                public string ResponseTxt => this._Response.ResponseTxt;


                public BasicHandler()
                {
                        this.SortNum = "0-0";
                }
                public BasicHandler(string uri, bool isRegex = false) : this(uri, int.MaxValue - uri.Length, isRegex)
                {

                }
                public BasicHandler(string uri, int sortNum, bool isRegex = false)
                {
                        this.RequestPath = uri.ToLower();
                        this._IsRegex = isRegex;
                        string num = sortNum.ToString().PadLeft(3, '0');
                        if (this._IsRegex)
                        {
                                this._Regex = new Regex(this.RequestPath, RegexOptions.IgnoreCase);
                                this.SortNum = string.Format("20-{0}", num);
                        }
                        else if (this.RequestPath.EndsWith("/"))
                        {
                                this.SortNum = string.Format("30-{0}", num);
                                this._IsComplate = false;
                        }
                        else
                        {
                                this.SortNum = string.Format("40-{0}", num);
                                this._IsComplate = true;
                        }
                }
                public void Init(HttpListenerContext context)
                {
                        this._Request = new HttpRequest(context.Request, this);
                        this._Response = new HttpResponse(context.Response, this);
                }
                /// <summary>
                /// 验证请求
                /// </summary>
                /// <returns></returns>
                public bool Verification()
                {
                        if (this.InitHandler())
                        {
                                if (!this.Request.InitRequest())
                                {
                                        return false;
                                }
                                else if (this._CheckCache())
                                {
                                        this.Response.SetHttpStatus(HttpStatusCode.NotModified);
                                        return false;
                                }
                                return true;
                        }
                        return false;
                }
                public bool IsEnd { get; private set; }



                protected virtual bool InitHandler()
                {
                        return true;
                }

                public virtual void End()
                {
                        this.IsEnd = true;
                }

                public virtual void Execute()
                {

                }
                public object Clone()
                {
                        return this.MemberwiseClone();
                }

                #region 缓存
                private bool _CheckCache()
                {
                        return (this.Request.Headers["If-Modified-Since"] != null || this.Request.Headers["If-None-Match"] != null) && this.CheckCache(this.Request.Headers["If-None-Match"], this.Request.Headers["If-Modified-Since"]);
                }

                protected virtual bool CheckCache(string etag, string toUpdateTime)
                {
                        return false;
                }
                #endregion

                public bool IsUsable(string path)
                {
                        if (this._IsComplate)
                        {
                                return path == this.RequestPath;
                        }
                        else if (this._IsRegex)
                        {
                                return this._Regex.IsMatch(path);
                        }
                        else
                        {
                                return path.StartsWith(this.RequestPath);
                        }
                }
                #region 文件上传

                public bool IsGenerateMd5 { get; protected set; }
                public virtual bool VerificationFile(UpFileParam param, long fileSize)
                {
                        return true;
                }
                public virtual bool CheckUpFile(UpFileParam param, int num)
                {
                        return true;
                }

                public void Dispose()
                {
                        this._Response.Dispose();
                }


                #endregion
        }
}
