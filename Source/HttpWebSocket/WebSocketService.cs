using System;

using HttpWebSocket.Collect;
using HttpWebSocket.Interface;

namespace HttpWebSocket
{
        public class WebSocketService
        {
                #region 添加服务端
                public static IService AddServer(IWebSocketConfig config)
                {
                        return ServerCollect.AddServer(config);
                }
                #endregion






        }
}
