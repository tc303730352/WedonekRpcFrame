using System.Net;
using System.Text;

using HttpService.Interface;

using RpcHelper;

namespace HttpService.Log
{
        internal class HttpLogHelper
        {
                private LogInfo _Log = null;
                private readonly HttpListenerRequest request = null;
                private readonly HttpListenerResponse response = null;
                private IBasicHandler _Handler = null;
                public HttpLogHelper(HttpListenerContext context)
                {
                        this.request = context.Request;
                        this.response = context.Response;
                }

                public void IntiLog()
                {
                        this._Log = HttpLog.CreateLog(this.request);
                }

                internal void Init(IBasicHandler handler)
                {
                        this._Handler = handler;
                        if (this.request.HttpMethod == "POST")
                        {
                                this._Log.Add("Post", handler.Request.PostString);
                        }
                        if (this.request.HttpMethod == "POST" && handler.Request.Files.Count != 0)
                        {
                                StringBuilder file = new StringBuilder();
                                handler.Request.Files.ForEach(i =>
                                {
                                        file.AppendFormat("\r\nFileName:{0};FileSize:{1};FileType:{2};ContentType:{3}\r\nTempFilePath:{4}", i.FileName, i.FileSize, i.FileType, i.ContentType);
                                });
                                this._Log.Add("File", file.ToString());
                        }
                }

                internal void SaveLog()
                {
                        this._Log.Add("StatusCode", this.response.StatusCode);
                        if (this.response.StatusCode == 200)
                        {
                                this._Log.Add("response_ContentType", this.response.ContentType);
                                this._Log.Add("response_ContentLen", this.response.ContentLength64);
                                this._Log.Add("response_Text", this._Handler.ResponseTxt);
                                if (this.response.Cookies.Count > 0)
                                {
                                        this._Log.Add("response_Cookie", this.response.Cookies.ToString());
                                }
                        }
                        this._Log.Save();
                }
        }
}
