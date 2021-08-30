using System;
using System.Net;

using HttpService.Collect;
using HttpService.Helper;
using HttpService.Interface;

using RpcHelper;

namespace HttpService.Server
{
        internal class Server : IHttpServer
        {
                public Server(Uri uri)
                {
                        this.Uri = uri;
                }
                public Server(Uri uri, string hash)
                {
                        this.Uri = uri;
                        this._CertHashVal = hash;
                }
                private readonly string _CertHashVal = null;
                public Uri Uri
                {
                        get;
                }

                private readonly HttpListener _HttpServer = new HttpListener();

                public void Start()
                {
                        if (this._InitServer())
                        {
                                ServiceCollect.AddServer(this);
                        }
                }


                private bool _InitServer()
                {
                        if (this.Uri.Scheme == "https")
                        {
                                if (!new HttpsTools(this._CertHashVal).BindUri(this.Uri))
                                {
                                        new WarnLog("http.cert.bind.fail", "https绑定失败!", "Uri:" + this.Uri, "Http").Save();
                                        return false;
                                }
                        }
                        this._HttpServer.Prefixes.Add(this.Uri.AbsoluteUri);
                        this._HttpServer.IgnoreWriteExceptions = false;
                        this._HttpServer.Start();
                        this._HttpServer.BeginGetContext(new AsyncCallback(this._Execution), null);
                        return true;
                }
                private void _Execution(IAsyncResult e)
                {
                        HttpListenerContext context = null;
                        try
                        {
                                context = this._HttpServer.EndGetContext(e);
                        }
                        catch (Exception ex)
                        {
                                new ErrorLog(ex, "执行错误!", "Uri:" + this.Uri, "Http").Save();
                        }
                        finally
                        {
                                this._HttpServer.BeginGetContext(new AsyncCallback(this._Execution), null);
                        }
                        ServiceCollect.Execution(this, context);
                }

                public void Close()
                {
                        this._HttpServer.Close();
                }
        }
}
