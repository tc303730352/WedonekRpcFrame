using WeDonekRpc.HttpService.Interface;

namespace WeDonekRpc.HttpApiGateway.Interface
{
    public interface IIdempotentHandler
    {
        bool CheckIsLimit(string path);
        string GetTokenId(IRoute route, IHttpHandler handler);
    }
}