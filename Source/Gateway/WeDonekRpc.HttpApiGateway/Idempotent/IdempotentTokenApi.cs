using WeDonekRpc.HttpApiGateway.Interface;

namespace WeDonekRpc.HttpApiGateway.Idempotent
{
    internal class IdempotentTokenApi : IApiGateway
    {
        private readonly ITokenIdempotent _Token;

        public IdempotentTokenApi (ITokenIdempotent token)
        {
            this._Token = token;
        }
        [ApiGateway.Attr.ApiStop]
        public string ApplyToken ()
        {
            return this._Token.ApplyToken();
        }
    }
}
