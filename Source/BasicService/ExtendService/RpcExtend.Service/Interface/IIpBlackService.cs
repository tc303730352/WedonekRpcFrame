using WeDonekRpc.Model;
using WeDonekRpc.ModularModel.IpBlack.Model;
namespace RpcExtend.Service.Interface
{
    public interface IIpBlackService
    {
        IpBlackList GetBlack (long ver, MsgSource source);
        void Refresh (long rpcMerId, string systemType);
    }
}