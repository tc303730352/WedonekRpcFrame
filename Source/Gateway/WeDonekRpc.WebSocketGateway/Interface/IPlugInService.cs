using WeDonekRpc.Client.Attr;

namespace WeDonekRpc.WebSocketGateway.Interface
{
    [ClassLifetimeAttr(ClassLifetimeType.SingleInstance)]
    public interface IPlugInService
    {
        void Add (IWebSocketPlugin plugIn);
        void Add (IWebSocketPlugin plugIn, int index);
        void Delete (string name);
    }
}
