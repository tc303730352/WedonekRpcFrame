using WeDonekRpc.TcpServer.Interface;

namespace WeDonekRpc.TcpServer.SystemAllot
{
    internal class PingAllot : IAllot
    {
        public override object Action ()
        {
            return "ok";
        }
    }
}
