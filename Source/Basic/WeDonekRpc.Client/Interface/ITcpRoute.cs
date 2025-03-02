using WeDonekRpc.Client.Model;

using WeDonekRpc.Model;

namespace WeDonekRpc.Client.Interface
{
    public delegate IBasicRes TcpMsgEvent (IMsg msg);


    internal interface ITcpRoute : IRoute
    {

    }
}
