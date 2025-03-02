namespace WeDonekRpc.ApiGateway.Interface
{
    public interface IIpLimitPlugIn : IPlugIn
    {
        bool IsLimit ( string ip );
    }
}