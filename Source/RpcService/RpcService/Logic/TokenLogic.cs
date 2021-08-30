using RpcModel;

using RpcService.Collect;
using RpcService.Controller;

namespace RpcService.Logic
{
        internal class TokenLogic
        {
                public static bool ApplyOAuthToken(ApplyAccessToken apply, out AccessTokenRes res, out string error)
                {
                        if (string.IsNullOrEmpty(apply.AppId))
                        {
                                res = null;
                                error = "rpc.appid.null";
                                return false;
                        }
                        else if (string.IsNullOrEmpty(apply.Secret))
                        {
                                res = null;
                                error = "rpc.secret.null";
                                return false;
                        }
                        else if (!Collect.RpcMerCollect.GetOAuthMer(apply.AppId, out RpcMerController mer))
                        {
                                res = null;
                                error = mer.Error;
                                return false;
                        }
                        else if (mer.AppSecret != apply.Secret)
                        {
                                res = null;
                                error = "rpc.secret.error";
                                return false;
                        }
                        else if (!RpcTokenCollect.ApplyOAuthToken(mer, apply.ServerId, out string tolenId, out error))
                        {
                                res = null;
                                return false;
                        }
                        else
                        {
                                res = new AccessTokenRes(tolenId, mer.OAuthMerId);
                                return true;
                        }
                }
        }
}
