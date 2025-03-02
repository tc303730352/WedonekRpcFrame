using WeDonekRpc.Client;
using WeDonekRpc.Client.Attr;
using WeDonekRpc.Helper.Config;
using WeDonekRpc.WebSocketGateway.Interface;

namespace WeDonekRpc.WebSocketGateway.Config
{
    [ClassLifetimeAttr(ClassLifetimeType.SingleInstance)]
    internal class SocketGatewayConfig : ISocketGatewayConfig
    {
        public SocketGatewayConfig ()
        {
            IConfigSection section = RpcClient.Config.GetSection("gateway:socket");
            section.AddRefreshEvent(this._Refresh);
        }

        private void _Refresh (IConfigSection config, string name)
        {
            this.RequestEncoding = config.GetValue<string>("RequestEncoding", "utf-8");
            this.ApiRouteFormat = config.GetValue<string>("ApiRouteFormat", "/api/{controller}/{name}");
        }

        /// <summary>
        /// 请求编码
        /// </summary>
        public string RequestEncoding
        {
            get;
            private set;
        } = "utf-8";
        /// <summary>
        /// 生成API访问路径格式
        /// </summary>
        public string ApiRouteFormat
        {
            get;
            private set;
        } = "/api/{controller}/{name}";

    }
}
