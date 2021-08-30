using RpcModel;

using RpcService.Logic;

namespace RpcService.Event
{
        internal class GetAccessTokenEvent : Route.TcpRoute<ApplyAccessToken, AccessTokenRes>
        {
                public GetAccessTokenEvent() : base("GetAccessToken")
                {

                }
                protected override bool ExecAction(ApplyAccessToken param, out AccessTokenRes result, out string error)
                {
                        return TokenLogic.ApplyOAuthToken(param, out result, out error);
                }
        }
}

