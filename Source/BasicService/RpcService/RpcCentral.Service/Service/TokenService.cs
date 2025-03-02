using RpcCentral.Collect;
using RpcCentral.Model.DB;
using RpcCentral.Service.Interface;
using WeDonekRpc.Helper;
using WeDonekRpc.Model.Server;

namespace RpcCentral.Service.Service
{
    internal class TokenService : ITokenService
    {
        private IRpcMerCollect _RpcMer;
        private IRpcTokenCollect _Token;

        public TokenService(IRpcMerCollect rpcMer, IRpcTokenCollect token)
        {
            this._RpcMer = rpcMer;
            this._Token = token;
        }

        public AccessTokenRes ApplyOAuthToken(ApplyAccessToken apply)
        {
            if (string.IsNullOrEmpty(apply.AppId))
            {
                throw new ErrorException("rpc.appid.null");
            }
            else if (string.IsNullOrEmpty(apply.Secret))
            {
                throw new ErrorException("rpc.secret.null");
            }
            else if (!this._RpcMer.GetMer(apply.AppId, out RpcMer mer, out string error))
            {
                throw new ErrorException(error);
            }
            else if (mer.AppSecret != apply.Secret)
            {
                throw new ErrorException("rpc.secret.error");
            }
            else
            {
                string tokenId = this._Token.Apply(mer, apply.ServerId);
                return new AccessTokenRes(tokenId, mer.Id);
            }
        }
    }
}
