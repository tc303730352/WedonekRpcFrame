using WeDonekRpc.Model.Server;

namespace RpcCentral.Service.Interface
{
    public interface IServerLoginService
    {
        RpcServerLoginRes Login(RpcServerLogin login, string remoteIp);
    }
}