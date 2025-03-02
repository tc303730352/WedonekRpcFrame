namespace WeDonekRpc.WebSocketGateway.Interface
{
    public interface ISocketGatewayConfig
    {
        /// <summary>
        /// Api 路由格式
        /// </summary>
        string ApiRouteFormat { get; }

        /// <summary>
        /// 清编码
        /// </summary>
        string RequestEncoding { get; }
    }
}