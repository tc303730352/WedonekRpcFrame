using WeDonekRpc.ApiGateway.Interface;
using WeDonekRpc.ModularModel.IpBlack;
using WeDonekRpc.ModularModel.IpBlack.Model;

namespace WeDonekRpc.ApiGateway.Service
{
    internal class IpBackService : IIpBackService
    {
        public IpBlackList GetIpBlack (long ver)
        {
            return new SyncIpBlack { LocalVer = ver }.Send();
        }
    }
}
