namespace WeDonekRpc.ApiGateway.Interface
{
    public interface IIpBlackListPlugIn : IPlugIn
    {

        bool IsLimit(string ip);
    }
}