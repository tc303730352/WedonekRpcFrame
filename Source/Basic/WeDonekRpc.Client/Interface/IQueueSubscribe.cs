namespace WeDonekRpc.Client.Interface
{
    internal interface IQueueSubscribe : System.IDisposable
    {
        void BindRoute (string routeKey);

        void Subscrib ();
    }
}