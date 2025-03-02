using RpcCentral.Service.Interface;
using WeDonekRpc.Model.Server;

namespace RpcCentral.Service.Event
{
    internal class GetAccessTokenEvent : Route.TcpRoute<ApplyAccessToken, AccessTokenRes>
    {
        private ITokenService _Token;
        public GetAccessTokenEvent(ITokenService token) : base()
        {
            _Token = token;
        }
        protected override AccessTokenRes ExecAction(ApplyAccessToken param)
        {
            return _Token.ApplyOAuthToken(param);
        }
    }
}

