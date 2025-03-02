using System.Text;
using WeDonekRpc.Helper.Config;
using WeDonekRpc.HttpWebSocket.Interface;
using WeDonekRpc.WebSocketGateway.Interface;

namespace WeDonekRpc.WebSocketGateway.Config
{
    [Client.Attr.IgnoreIoc]
    internal class ModularConfig : IModularConfig
    {

        private readonly IConfigSection _Config;
        public ModularConfig (IApiModular modular)
        {
            this._Config = LocalConfig.Local.GetSection(string.Concat("gateway:", modular.ServiceName));
            this.IsAccredit = this._Config.GetValue("IsAccredit", true);
            this.IsIdentity = this._Config.GetValue("IsIdentity", false);
            string encoding = this._Config.GetValue("RequestEncoding", GatewayService.Config.RequestEncoding);
            if (encoding != this.RequestEncoding.BodyName)
            {
                this.RequestEncoding = Encoding.GetEncoding(encoding);
            }
            this.SocketConfig = new HttpWebSocket.Config.WebSocketConfig(new WebSocketEvent(modular), this._Config);
            this.ApiRouteFormat = this._Config.GetValue("ApiRouteFormat", GatewayService.Config.ApiRouteFormat);
            this._Config.AddRefreshEvent(this._Refresh);
        }

        private void _Refresh (IConfigSection config, string name)
        {
            if (name == "IsAccredit" || name == "RequestEncoding" || name == "IsIdentity")
            {
                this.IsIdentity = this._Config.GetValue("IsIdentity", false);
                this.IsAccredit = config.GetValue("IsAccredit", true);
                string encoding = config.GetValue("RequestEncoding", GatewayService.Config.RequestEncoding);
                if (encoding != this.RequestEncoding.BodyName)
                {
                    this.RequestEncoding = Encoding.GetEncoding(encoding);
                }
            }
        }
        public IWebSocketConfig SocketConfig
        {
            get;
        }
        /// <summary>
        ///  Api 接口路径生成格式
        /// </summary>
        public string ApiRouteFormat
        {
            get;
            set;
        }
        public IResponseTemplate ResponseTemplate
        {
            get;
            set;
        } = new ResponseTemplate();

        public Encoding RequestEncoding
        {
            get;
            set;
        } = Encoding.UTF8;

        public bool IsAccredit { get; private set; }

        public bool IsIdentity { get; private set; }
    }
}
