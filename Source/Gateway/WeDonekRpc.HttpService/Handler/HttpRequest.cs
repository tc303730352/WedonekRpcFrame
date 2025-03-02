using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using WeDonekRpc.HttpService.FileUp;
using WeDonekRpc.HttpService.Helper;
using WeDonekRpc.HttpService.Interface;

namespace WeDonekRpc.HttpService.Handler
{
    /// <summary>
    /// Http请求对象
    /// </summary>
    internal class HttpRequest : IHttpRequest
    {
        private readonly HttpListenerRequest _Request = null;
        private readonly IBasicHandler _Handler = null;
        private string _ClientIp = null;
        private NameValueCollection _Form = null;
        private string _PostStr = null;
        private byte[] _RequestStream = null;
        private Uri _UrlReferrer = null;

        internal HttpRequest ( HttpListenerRequest request, IBasicHandler handler )
        {
            this._Handler = handler;
            this._Request = request;
        }
        public void InitRequest ()
        {
            if ( Helper.Helper.CheckIsUpFile(this._Request.ContentType) )
            {
                this._PostStr = string.Empty;
                this.IsPostFile = true;
                this.Files = [];
                this._Handler.InitFileUp();
                new SyncUpFileHelper(this).LoadFile(this._Request.InputStream, this._Handler);
            }
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
                if ( this._RequestStream == null && this._Request.HttpMethod == "POST" && this._Request.ContentLength64 != 0 && this._Request.InputStream != null )
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
                if ( !this.IsPostFile && this._PostStr == null && this.Stream != null )
                {
                    this._PostStr = HttpService.Config.RequestEncoding.GetString(this.Stream).Replace("\r", "").Replace("\n", "").Replace("\0", "").Trim();
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
                if ( this._UrlReferrer == null )
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
                this._ClientIp ??= this._Request.ToClientIp();
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
                this._Form ??= this._Request.HttpMethod == "POST" && this.PostString != null ? Helper.Helper.GetForm(this.PostString) : ( [] );
                return this._Form;
            }
        }

        public string ContentType => this._Request.ContentType;

        public long ContentLength => this._Request.ContentLength64;

        public string HttpMethod => this._Request.HttpMethod;

        public List<IUpFile> Files { get; private set; } = null;


        /// <summary>
        /// 获取Cookie
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Cookie GetCookie ( string name )
        {
            return this._Request.Cookies[name];
        }

        public string GetCookieValue ( string name )
        {
            Cookie cookie = this.GetCookie(name);
            return cookie?.Value;
        }

        internal void AddFile ( IUpFile file )
        {
            this.Files.Add(file);
        }
        internal void SetPostStr ( string form )
        {
            this._PostStr = form;
        }
        public void Dispose ()
        {
            if ( this.IsPostFile )
            {
                this.Files.ForEach(a => a.Dispose());
            }
        }
    }
}
