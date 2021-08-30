using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;

using HttpService.Config;
using HttpService.Interface;

using RpcHelper;

namespace HttpService.Handler
{
        /// <summary>
        /// Http请求对象
        /// </summary>
        internal class HttpRequest : IHttpRequest, IUpFileRequest
        {
                private readonly HttpListenerRequest _Request = null;
                private readonly IBasicHandler _Handler = null;

                private string _ClientIp = null;

                private string _TempFileSavePath = null;
                private List<IUpFile> _Files = null;

                private NameValueCollection _Form = null;
                private string _PostStr = null;
                private byte[] _RequestStream = null;
                private Uri _UrlReferrer = null;

                internal HttpRequest(HttpListenerRequest request, IBasicHandler handler)
                {
                        this._Handler = handler;
                        this._Request = request;
                }
                public bool InitRequest()
                {
                        if (!Helper.Helper.CheckIsUpFile(this._Request.ContentType))
                        {
                                return true;
                        }
                        this._PostStr = string.Empty;
                        this.IsPostFile = true;
                        this._Files = new List<IUpFile>();
                        return new SyncUpFileHelper(this).LoadFile(this._Request.InputStream, this);
                }
                /// <summary>
                /// 是否是文件上传请求
                /// </summary>
                public bool IsPostFile { get; private set; }
                /// <summary>
                /// 请求地址
                /// </summary>
                public Uri Url => this._Request.Url;


                /// <summary>
                /// 请求头
                /// </summary>
                public NameValueCollection Headers => this._Request.Headers;
                /// <summary>
                /// Get查询
                /// </summary>
                public NameValueCollection QueryString => this._Request.QueryString;

                /// <summary>
                /// 请求流
                /// </summary>
                public byte[] Stream
                {
                        get
                        {
                                if (this._RequestStream == null && this._Request.HttpMethod == "POST" && this._Request.ContentLength64 != 0 && this._Request.InputStream != null)
                                {
                                        this._RequestStream = Helper.Helper.ReadLineAsBytes(this._Request.InputStream, (int)this._Request.ContentLength64);
                                }
                                return this._RequestStream;
                        }
                }

                /// <summary>
                /// POST字符串
                /// </summary>
                public string PostString
                {
                        get
                        {
                                if (this._PostStr == null && this.Stream != null)
                                {
                                        this._PostStr = Encoding.UTF8.GetString(this.Stream).Replace("\r", "").Replace("\n", "").Replace("\0", "").Trim();
                                }
                                return this._PostStr;
                        }
                }

                /// <summary>
                /// 请求来源
                /// </summary>
                public Uri UrlReferrer
                {
                        get
                        {
                                if (this._UrlReferrer == null)
                                {
                                        this._UrlReferrer = Helper.Helper.GetUrlReferrer(this._Request);
                                }
                                return this._UrlReferrer;
                        }
                }

                /// <summary>
                /// 客户端IP
                /// </summary>
                public string ClientIp
                {
                        get
                        {
                                if (this._ClientIp == null)
                                {
                                        this._ClientIp = this._Request.RemoteEndPoint.Address.ToString();
                                }
                                return this._ClientIp;
                        }
                }

                /// <summary>
                /// 提交的表单
                /// </summary>
                public NameValueCollection Form
                {
                        get
                        {
                                if (this._Form == null)
                                {
                                        if (this._Request.HttpMethod == "POST" && this.PostString != null)
                                        {
                                                this._Form = Helper.Helper.GetForm(this.PostString);
                                        }
                                        else
                                        {
                                                this._Form = new NameValueCollection();
                                        }
                                }
                                return this._Form;
                        }
                }

                public string ContentType => this._Request.ContentType;

                public long ContentLength => this._Request.ContentLength64;

                public string HttpMethod => this._Request.HttpMethod;

                public List<IUpFile> Files => this._Files;

                public bool IsGenerateMd5 => this._Handler.IsGenerateMd5;

                public T GetPostObject<T>() where T : class
                {
                        return string.IsNullOrEmpty(this.PostString) ? default : Tools.Json<T>(this.PostString);
                }
                /// <summary>
                /// 获取Cookie
                /// </summary>
                /// <param name="name"></param>
                /// <returns></returns>
                public Cookie GetCookie(string name)
                {
                        return this._Request.Cookies[name];
                }

                public string GetCookieValue(string name)
                {
                        Cookie cookie = this.GetCookie(name);
                        return cookie?.Value;
                }

                public string GetLocalUri(string path)
                {
                        return Helper.FileHelper.GetFileUri(this.Url, path);
                }
                /// <summary>
                ///获取请求的本地路径
                /// </summary>
                /// <returns></returns>
                public string GetLocalPath()
                {
                        return string.Format("{0}{1}", ServerConfig.FileSavePath, this._Request.Url.AbsolutePath.Replace("/", "\\"));
                }
                #region 文件上传

                public bool CheckUpFile(UpFileParam param)
                {
                        return this._Handler.CheckUpFile(param, this.Files.Count + 1);
                }

                public Stream GetSaveStream(UpFileParam param)
                {
                        string path = Helper.Helper.GetUpTempFileSavePath(param);
                        if (string.IsNullOrEmpty(path))
                        {
                                return null;
                        }
                        this._TempFileSavePath = string.Concat(path, ".tmp");
                        FileInfo file = new FileInfo(this._TempFileSavePath);
                        if (!file.Directory.Exists)
                        {
                                file.Directory.Create();
                        }
                        return file.Open(FileMode.Create, FileAccess.ReadWrite, FileShare.None);
                }

                /// <summary>
                /// 保存文件
                /// </summary>
                /// <param name="upParam"></param>
                /// <param name="stream"></param>
                public void SaveFile(UpFileParam upParam, Stream stream)
                {
                        if (upParam.Name == "description")
                        {
                                byte[] myByte = ((MemoryStream)stream).ToArray();
                                this._PostStr = Encoding.UTF8.GetString(myByte);
                        }
                        else
                        {
                                this._Files.Add(new UpFile(upParam, this._TempFileSavePath, stream.Length));
                        }
                }

                public void UpFail()
                {
                        if (this._TempFileSavePath != null)
                        {
                                File.Delete(this._TempFileSavePath);
                        }
                }

                public void SetForm(string form)
                {
                        this._PostStr = form;
                }

                public bool VerificationFile(UpFileParam upParam, long length)
                {
                        return this._Handler.VerificationFile(upParam, length);
                }
                #endregion
        }
}
