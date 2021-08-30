namespace WebSocketGateway.Interface
{
        internal interface ISocketApi
        {
                string LocalPath { get; }

                void ExecApi(IWebSocketService service);
                void RegApi(IApiRoute apiRoute);
                bool VerificationApi();
        }
}
