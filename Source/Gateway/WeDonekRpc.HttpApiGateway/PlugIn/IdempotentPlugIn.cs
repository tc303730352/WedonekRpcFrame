using WeDonekRpc.HttpApiGateway.Idempotent;
using WeDonekRpc.HttpApiGateway.Interface;
using WeDonekRpc.HttpService.Interface;
using WeDonekRpc.Helper;

namespace WeDonekRpc.HttpApiGateway.PlugIn
{
    /// <summary>
    /// 请求去幂等
    /// </summary>
    internal class IdempotentPlugIn : IHttpPlugIn
    {
        private readonly IIdempotent _Idem;
        private IIdempotentHandler _Handler;
        public IdempotentPlugIn(IIdempotent idem, IIdempotentHandler handler)
        {
            _Idem = idem;
            _Handler = handler;
        }

        public string Name => "Idempotent";

        public bool IsEnable => true;

        public void Dispose()
        {
            _Idem.Dispose();
        }

        public void Exec(IRoute route, IHttpHandler handler)
        {
            string tokenId = this._Handler.GetTokenId(route, handler);
            if(tokenId.IsNull() && this._Handler.CheckIsLimit(route.ApiUri))
            {
                if (this._Handler.CheckIsLimit(route.ApiUri))
                {
                    handler.Response.SetHttpStatus(System.Net.HttpStatusCode.Unauthorized);
                }
            }
            else if (!_Idem.SubmitToken(tokenId))
            {
                handler.Response.SetHttpStatus(System.Net.HttpStatusCode.Forbidden);
            }
        }

        public void Init()
        {
        }
    }
}
