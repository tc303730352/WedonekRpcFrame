using WeDonekRpc.Model.Server;

namespace RpcCentral.Service.Interface
{
    public interface IContrainerLoginService
    {
        RpcServerLoginRes Login(RpcServerLogin login, string remoteIp);
    }
}