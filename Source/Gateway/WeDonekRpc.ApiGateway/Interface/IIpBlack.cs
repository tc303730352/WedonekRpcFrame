namespace WeDonekRpc.ApiGateway.Interface
{
    internal interface IIpBlack : System.IDisposable
    {
        bool IsInit { get; }
        void Init (IIpBlackConfig config);
        bool IsLimit (string ip);
    }
}