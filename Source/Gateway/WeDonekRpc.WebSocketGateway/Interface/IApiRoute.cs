namespace WeDonekRpc.WebSocketGateway.Interface
{
    public interface IApiRoute : IRoute
    {
        void ExecApi (IWebSocketService service);
        void RegApi ();
    }
}
