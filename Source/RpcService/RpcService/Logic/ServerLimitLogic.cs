using RpcModel.Model;
using RpcModel.Server;

using RpcService.Collect;
using RpcService.Model;

namespace RpcService.Logic
{
        internal class ServerLimitLogic
        {
                internal static bool GetServerLimit(GetServerLimit obj, out LimitConfigRes result, out string error)
                {
                        if (!RpcTokenCollect.GetOAuthToken(obj.AccessToken, out RpcToken token, out error))
                        {
                                result = null;
                                return false;
                        }
                        else if (!ServerLimitConfigCollect.GetServerLimit(token.ConServerId, out ServerLimitConfig config, out error))
                        {
                                result = null;
                                return false;
                        }
                        else if (!ServerDictateLimitCollect.GetDictateLimit(token.ConServerId, out ServerDictateLimit[] limits, out error))
                        {
                                result = null;
                                return false;
                        }
                        else
                        {
                                result = new LimitConfigRes
                                {
                                        LimitConfig = config,
                                        DictateLimit = limits
                                };
                                return true;
                        }
                }
        }
}
