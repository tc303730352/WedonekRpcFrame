using System.Text;
using WeDonekRpc.HttpWebSocket.Interface;

namespace WeDonekRpc.WebSocketGateway.Interface
{
    /// <summary>
    /// 模块配置
    /// </summary>
    public interface IModularConfig
    {
        /// <summary>
        /// 是否认证
        /// </summary>
        bool IsAccredit { get; }

        /// <summary>
        /// 是否启用身份标识
        /// </summary>
        bool IsIdentity { get; }
        /// <summary>
        ///   Api 接口地址生成格式
        /// </summary>
        string ApiRouteFormat { get; }
        /// <summary>
        /// Socket配置
        /// </summary>
        IWebSocketConfig SocketConfig { get; }

        /// <summary>
        /// 响应模板
        /// </summary>
        IResponseTemplate ResponseTemplate { get; set; }
        /// <summary>
        /// 请求编码
        /// </summary>
        Encoding RequestEncoding { get; }

    }
}