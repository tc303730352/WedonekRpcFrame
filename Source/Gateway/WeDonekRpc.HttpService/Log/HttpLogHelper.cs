using System.Net;
using System.Text;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Log;
using WeDonekRpc.HttpService.Config;
using WeDonekRpc.HttpService.Interface;

namespace WeDonekRpc.HttpService.Log
{
    internal class HttpLogHelper
    {
        private LogInfo _Log = null;
        private readonly LogConfig _Config;
        private readonly HttpListenerRequest request = null;
        private readonly HttpListenerResponse response = null;
        private IBasicHandler _Handler = null;
        public HttpLogHelper (HttpListenerContext context, LogConfig config)
        {
            this._Config = config;
            this.request = context.Request;
            this.response = context.Response;
        }

        public void IntiLog ()
        {
            this._Log = HttpLog.CreateLog(this.request, this._Config);
        }

        internal void Init (IBasicHandler handler)
        {
            this._Handler = handler;
            if (this.request.HttpMethod == "POST")
            {
                this._Log.Add("Post", handler.Request.PostString);
            }
            if (this.request.HttpMethod == "POST" && handler.Request.Files != null && handler.Request.Files.Count != 0)
            {
                StringBuilder file = new StringBuilder();
                handler.Request.Files.ForEach(i =>
                {
                    _ = file.AppendFormat("\r\nFileName:{0};FileSize:{1};FileType:{2};ContentType:{3}\r\nTempFilePath:{4}", i.FileName, i.FileSize, i.FileType, i.ContentType);
                });
                this._Log.Add("File", file.ToString());
            }
        }

        internal void SaveLog ()
        {
            this._Log.Add("StatusCode", this.response.StatusCode);
            if (this.response.StatusCode == 200)
            {
                this._Log.Add("response_ContentType", this.response.ContentType);
                this._Log.Add("response_ContentLen", this.response.ContentLength64);
                if (this._Handler != null)
                {
                    this._Log.Add("response_Text", this._Handler.ResponseTxt);
                }
                if (this.response.Cookies.Count > 0)
                {
                    this._Log.Add("response_Cookie", this.response.Cookies.ToString());
                }
            }
            else if (this.response.StatusCode == 302)
            {
                this._Log.Add("response_Location", this.response.Headers["Location"]);
            }
            if (!this._Config.ResponseHead.IsNull())
            {
                StringBuilder str = new StringBuilder();
                this._Config.ResponseHead.ForEach(c =>
                {
                    string val = this.response.Headers[c];
                    if (val != null)
                    {
                        _ = str.AppendFormat("{0}={1};", c, val);
                    }
                });
                this._Log.Add("response_head", str.ToString());
            }
            this._Log.Save();
        }
    }
}
