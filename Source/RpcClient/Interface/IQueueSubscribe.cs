namespace RpcClient.Interface
{
        internal interface IQueueSubscribe : System.IDisposable
        {
                void BindRoute(string routeKey);

                void Subscrib();
        }
}