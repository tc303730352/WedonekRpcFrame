using RpcCentral.Service.Model;
using WeDonekRpc.Model;

namespace RpcCentral.Service.Interface
{
    public delegate IBasicRes TcpMsgEvent(RemoteMsg msg);
    public interface ITcpRoute
    {
        TcpMsgEvent TcpMsgEvent { get; }
    }
}