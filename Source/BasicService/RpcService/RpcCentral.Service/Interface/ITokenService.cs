using WeDonekRpc.Model.Server;

namespace RpcCentral.Service.Interface
{
    public interface ITokenService
    {
        AccessTokenRes ApplyOAuthToken(ApplyAccessToken apply);
    }
}