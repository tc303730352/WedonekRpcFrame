using WeDonekRpc.ModularModel.IpBlack.Model;

namespace WeDonekRpc.ApiGateway.Interface
{
    public interface IIpBackService
    {
        IpBlackList GetIpBlack (long ver);
    }
}