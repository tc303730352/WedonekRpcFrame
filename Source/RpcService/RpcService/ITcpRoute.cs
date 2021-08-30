using RpcModel;

using RpcService.Model;

namespace RpcService
{
        internal delegate IBasicRes TcpMsgEvent(RemoteMsg msg);
        internal interface ITcpRoute
        {
                string RouteName { get; }
                TcpMsgEvent TcpMsgEvent { get; }
        }
}