using WeDonekRpc.HttpWebSocket.Collect;
using WeDonekRpc.HttpWebSocket.Interface;

namespace WeDonekRpc.HttpWebSocket
{
    public class WebSocketService
    {
        #region 添加服务端
        public static IService AddServer (IWebSocketConfig config)
        {
            return ServerCollect.AddServer(config);
        }
        #endregion


    }
}
