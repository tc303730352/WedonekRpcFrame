using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using WeDonekRpc.HttpService.FileUp;
using WeDonekRpc.HttpService.Interface;
using WeDonekRpc.HttpService.Model;

namespace WeDonekRpc.HttpService.Handler
{
    public class BasicHandler : IBasicHandler
    {
        private string _TempFileSavePath = null;
        private HttpRequest _Request = null;
        private readonly RequestPathType _PathType;
        private readonly KeyValuePair<int, string>[] _RuleKeys;
        private readonly string[] _RulePath;
        /// <summary>
        /// 请求对象
        /// </summary>
        public IHttpRequest Request => this._Request;


        /// <summary>
        /// 响应对象
        /// </summary>
        public IHttpResponse Response { get; private set; } = null;

        public int SortNum { get; }


        public bool IsFullPath => this._PathType == RequestPathType.Full;

        private readonly Regex _Regex = null;

        public bool IsRegex => this._PathType == RequestPathType.Regex;

        /// <summary>
        /// 请求路径
        /// </summary>
        public string RequestPath
        {
            get;
        }
        public string ResponseTxt => this.Response.ResponseTxt;

        public Dictionary<string, string> RouteArgs
        {
            get;
            private set;
        }
        public Dictionary<string, string> PathArgs
        {
            get;
            internal set;
        }
        public BasicHandler ()
        {
            this.SortNum = 0;
        }
        public BasicHandler (string uri, bool isRegex = false) : this(uri, (short)( short.MaxValue - uri.Length ), isRegex)
        {

        }
        public BasicHandler (string uri, short sortNum, bool isRegex = false)
        {
            this.RequestPath = uri.ToLower();
            if (isRegex)
            {
                this._PathType = RequestPathType.Regex;
                this._Regex = new Regex(this.RequestPath, RegexOptions.IgnoreCase);
                this.SortNum = 200000 + sortNum;
            }
            else
            {
                this._PathType = Helper.Helper.GetPathType(this.RequestPath, out this._RuleKeys, out this._RulePath);
                if (this._PathType == RequestPathType.Full)
                {
                    this.SortNum = 500000 + sortNum;
                }
                else if (this._PathType == RequestPathType.Relative)
                {
                    this.SortNum = 400000 + sortNum;
                }
                else
                {
                    this.SortNum = 300000 + sortNum;
                }
            }
        }
        public void Init (HttpListenerContext context, Dictionary<string, string> routeArgs)
        {
            this.RouteArgs = routeArgs;
            this._Request = new HttpRequest(context.Request, this);
            this.Response = new HttpResponse(context.Response, this);
            if (this._PathType == RequestPathType.RulePath)
            {
                Helper.Helper.SetRuleQuery(this, this._RuleKeys);
            }
        }
        /// <summary>
        /// 验证请求
        /// </summary>
        /// <returns></returns>
        public bool Verification ()
        {
            if (this.InitHandler())
            {
                this.Request.InitRequest();
                if (this._CheckCache())
                {
                    this.Response.SetHttpStatus(HttpStatusCode.NotModified);
                    return false;
                }
                return true;
            }
            return false;
        }
        public bool IsEnd { get; private set; }



        protected virtual bool InitHandler ()
        {
            return true;
        }

        public virtual void End ()
        {
            this.IsEnd = true;
        }

        public virtual void Execute ()
        {

        }
        public object Clone ()
        {
            return this.MemberwiseClone();
        }

        #region 缓存
        private bool _CheckCache ()
        {
            return ( this.Request.Headers["If-Modified-Since"] != null || this.Request.Headers["If-None-Match"] != null ) && this.CheckCache(this.Request.Headers["If-None-Match"], this.Request.Headers["If-Modified-Since"]);
        }

        protected virtual bool CheckCache (string etag, string toUpdateTime)
        {
            return false;
        }
        #endregion

        public bool IsUsable (string path)
        {
            if (this._PathType == RequestPathType.Relative)
            {
                return path.StartsWith(this.RequestPath);
            }
            else if (this._PathType == RequestPathType.RulePath)
            {
                string[] paths = path.Split('/');
                return Helper.Helper.CheckRulePath(paths, this._RulePath);
            }
            else if (this._PathType == RequestPathType.Regex)
            {
                return this._Regex.IsMatch(path);
            }
            return false;
        }
        #region 文件上传

        public bool IsGenerateMd5 { get; protected set; }


        /// <summary>
        /// 验证文件
        /// </summary>
        /// <param name="param"></param>
        /// <param name="fileSize"></param>
        public virtual void VerificationFile (UpFileParam param, long fileSize)
        {
        }
        /// <summary>
        /// 检查上传文件资料
        /// </summary>
        /// <param name="param"></param>
        public virtual void CheckUpFile (UpFileParam param)
        {
        }

        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="upParam"></param>
        /// <param name="stream"></param>
        public void SaveUpStream (UpFileParam upParam, Stream stream)
        {
            if (upParam.Name == "description")
            {
                byte[] myByte = ( (MemoryStream)stream ).ToArray();
                this._Request.SetPostStr(Encoding.UTF8.GetString(myByte));
            }
            else
            {
                IUpFile file = this._GetSaveFile(upParam, stream);
                this._Request.AddFile(file);
            }
        }

        public virtual void UpFail (Exception e)
        {
        }

        public virtual void Dispose ()
        {
            this._Request.Dispose();
            this.Response.Dispose();
        }

        public virtual Stream GetSaveStream (UpFileParam param)
        {
            if (HttpService.Config.Up.MemoryUpSize < this.Request.ContentLength)
            {
                return new MemoryStream((int)this.Request.ContentLength);
            }
            string path = this.ApplyTempSavePath(param);
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
        private IUpFile _GetSaveFile (UpFileParam upParam, Stream stream)
        {
            if (this._TempFileSavePath == null)
            {
                return new PostFile(upParam, stream);
            }
            return new UpFile(upParam, this._TempFileSavePath, stream.Length);
        }

        public virtual string ApplyTempSavePath (UpFileParam param)
        {
            return Helper.Helper.GetUpTempFileSavePath(param);
        }
        public virtual void InitFileUp ()
        {
        }


        #endregion

        public virtual void ExecError (Exception error)
        {
            HttpLog.AddErrorLog(this.Request.Url, error);
            this.Response.SetHttpStatus(HttpStatusCode.InternalServerError);
        }
    }
}
