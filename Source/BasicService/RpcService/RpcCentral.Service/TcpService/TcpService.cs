using WeDonekRpc.TcpServer;
using WeDonekRpc.TcpServer.Config;

namespace RpcCentral.Service.TcpService
{
    internal class TcpService
    {
        public static void Start ()
        {
            SocketConfig.DefaultAllot = new TcpAllot();
            SocketConfig.DefaultServerPort = 983;
            SocketConfig.SocketEvent = new TcpSocketEvent();
            TcpServer.Init();
        }
    }
}
