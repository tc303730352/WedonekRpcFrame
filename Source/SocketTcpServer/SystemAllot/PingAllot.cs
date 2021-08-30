using SocketTcpServer.Interface;

namespace SocketTcpClient.SystemAllot
{
        internal class PingAllot : IAllot
        {
                public override object Action()
                {
                        return "ok";
                }
        }
}
