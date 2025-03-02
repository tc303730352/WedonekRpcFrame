namespace WeDonekRpc.HttpApiGateway.Interface
{
    internal interface IResourceCollect : System.IDisposable
    {
        void RegRoute(IRoute route);
    }
}