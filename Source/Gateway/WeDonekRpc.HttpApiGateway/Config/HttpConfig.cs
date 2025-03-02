using System;
using WeDonekRpc.Client;
using WeDonekRpc.Client.Attr;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Config;
using WeDonekRpc.HttpApiGateway.Interface;
namespace WeDonekRpc.HttpApiGateway.Config
{
    [ClassLifetimeAttr(ClassLifetimeType.SingleInstance)]
    internal class HttpConfig : IHttpConfig
    {
        private Action<IHttpConfig, string> _refreshEvent;
        public HttpConfig ()
        {
            this.UpConfig = new GatewayUpConfig();
            RpcClient.Config.GetSection("gateway:http:server").AddRefreshEvent(this._RefreshHttp);
        }
        private void _RefreshHttp ( IConfigSection config, string name )
        {
            this.Url = config.GetValue("Url");
            this.RealRequestUri = config.GetValue<Uri>("RealRequestUri");
            this.MaxRequstLength = config.GetValue<long>("MaxRequstLength");
            this.IsEnableLongToString = config.GetValue<bool>("IsLongToString", true);
            this.ApiRouteFormat = config.GetValue<string>("ApiRouteFormat", "/api/{controller}/{name}");
            if ( this.RealRequestUri == null && this.Url.Validate(WeDonekRpc.Helper.Validate.ValidateFormat.URL) )
            {
                this.RealRequestUri = new Uri(this.Url);
            }
            if ( name != string.Empty )
            {
                this._refreshEvent?.Invoke(this, name);
            }
        }

        public bool CheckContentLen ( long len )
        {
            return this.MaxRequstLength == 0 || len < this.MaxRequstLength;
        }

        public void RefreshEvent ( Action<IHttpConfig, string> action )
        {
            this._refreshEvent += action;
            action(this, null);
        }

        /// <summary>
        /// Api响应模板
        /// </summary>
        public IApiResponseTemplate ApiTemplate
        {
            get;
            set;
        } = new Response.ApiResponseTemplate();
        /// <summary>
        /// 网关地址
        /// </summary>
        public string Url { get; private set; }

        public string ApiRouteFormat { get; private set; }
        /// <summary>
        /// 最大请求长度
        /// </summary>
        public long MaxRequstLength { get; private set; }

        public IGatewayUpConfig UpConfig { get; private set; }

        public Uri RealRequestUri { get; private set; }
        /// <summary>
        /// 是否启用JSON 序列化时 Long 转 String
        /// </summary>
        public bool IsEnableLongToString { get; private set; }
    }
}
