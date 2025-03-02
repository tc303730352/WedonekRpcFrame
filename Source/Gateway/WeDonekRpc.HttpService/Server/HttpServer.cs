using System;
using System.Net;
using System.Runtime.InteropServices;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Log;
using WeDonekRpc.HttpService.Collect;
using WeDonekRpc.HttpService.Helper;
using WeDonekRpc.HttpService.Interface;

namespace WeDonekRpc.HttpService.Server
{
    internal class Server : IHttpServer
    {
        private readonly string _CertHashVal = null;
        private readonly string _BindUri;
        private readonly IHttpConfig _Config;
        public Server ( string uri, string hash, IHttpConfig config )
        {
            this._Config = config;

            if ( config.RealRequestUri != null )
            {
                this.Uri = config.RealRequestUri;
            }
            else if ( uri.Validate(WeDonekRpc.Helper.Validate.ValidateFormat.URL) )
            {
                this.Uri = new Uri(uri);
            }
            this._BindUri = uri;
            this._CertHashVal = hash;
        }
        public Uri Uri
        {
            get;
        }

        private readonly HttpListener _HttpServer = new HttpListener();

        public void Start ()
        {
            if ( this._InitServer() )
            {
                ServiceCollect.AddServer(this);
            }
        }


        private bool _InitServer ()
        {
            if ( this._BindUri.StartsWith("https") && this.Uri != null )
            {
                if ( !new HttpsTools(this._CertHashVal).BindUri(this.Uri) )
                {
                    new WarnLog("http.cert.bind.fail", "https绑定失败!", "Uri:" + this._BindUri, "Http").Save();
                    return false;
                }
            }
            this._HttpServer.Prefixes.Add(this._BindUri);
            this._HttpServer.IgnoreWriteExceptions = this._Config.IgnoreWriteExceptions;
            this._HttpServer.Realm = this._Config.Realm;
            this._HttpServer.AuthenticationSchemes = this._Config.AuthenticationSchemes;
            this._HttpServer.TimeoutManager.IdleConnection = this._Config.TimeOut.IdleConnection;
            this._HttpServer.TimeoutManager.DrainEntityBody = this._Config.TimeOut.DrainEntityBody;
            if ( RuntimeInformation.IsOSPlatform(OSPlatform.Windows) )
            {
                this._HttpServer.TimeoutManager.EntityBody = this._Config.TimeOut.EntityBody;
                this._HttpServer.TimeoutManager.RequestQueue = this._Config.TimeOut.RequestQueue;
                this._HttpServer.TimeoutManager.MinSendBytesPerSecond = this._Config.TimeOut.MinSendBytesPerSecond;
                this._HttpServer.TimeoutManager.HeaderWait = this._Config.TimeOut.HeaderWait;
            }
            this._HttpServer.Start();
            _ = this._HttpServer.BeginGetContext(new AsyncCallback(this._Execution), null);
            return true;
        }
        private void _Execution ( IAsyncResult e )
        {
            HttpListenerContext context = null;
            try
            {
                context = this._HttpServer.EndGetContext(e);
            }
            catch ( Exception ex )
            {
                new ErrorLog(ex, "执行错误!", "Uri:" + this.Uri, "Http").Save();
            }
            finally
            {
                _ = this._HttpServer.BeginGetContext(new AsyncCallback(this._Execution), null);
            }
            ServiceCollect.Execution(this, context);
        }

        public void Close ()
        {
            this._HttpServer.Close();
        }
    }
}
