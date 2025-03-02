namespace WeDonekRpc.WebSocketGateway.Interface
{
    internal interface IResourceCollect : System.IDisposable
    {
        void RegRoute (ApiHandler route);
    }
}