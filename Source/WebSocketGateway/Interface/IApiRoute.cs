namespace WebSocketGateway.Interface
{
        internal interface IApiRoute: IRoute
        {
                void ExecApi(IWebSocketService service);
                void RegApi();
        }
}
