using System.Collections.Concurrent;

using RpcService.Controller;

using RpcHelper;

namespace RpcService.Collect
{
        internal class RpcMerCollect
        {
                private static readonly ConcurrentDictionary<string, RpcMerController> _OAuthMerList = new ConcurrentDictionary<string, RpcMerController>();
                static RpcMerCollect()
                {
                        AutoClearDic.AutoClear(_OAuthMerList);
                }


                public static bool Refresh(long id, out string error)
                {
                        if (!_GetMerAppId(id, out string appId, out error))
                        {
                                return false;
                        }
                        else if (_OAuthMerList.TryRemove(appId, out RpcMerController mer))
                        {
                                mer.Dispose();
                        }
                        return true;
                }

                public static bool GetOAuthMer(string appid, out RpcMerController mer)
                {
                        if (!_OAuthMerList.TryGetValue(appid, out mer))
                        {
                                mer = _OAuthMerList.GetOrAdd(appid, new RpcMerController(appid));
                        }
                        if (!mer.Init())
                        {
                                _OAuthMerList.TryRemove(appid, out mer);
                                mer.Dispose();
                                return false;
                        }
                        return mer.IsInit;
                }
                public static bool GetMer(long id, out RpcMerController mer, out string error)
                {
                        if (!_GetMerAppId(id, out string appId, out error))
                        {
                                mer = null;
                                return false;
                        }
                        else if (!GetOAuthMer(appId, out mer))
                        {
                                error = mer.Error;
                                return false;
                        }
                        return true;
                }
                private static bool _GetMerAppId(long id, out string appId, out string error)
                {
                        string key = string.Concat("RpcMer_", id);
                        if (RpcService.Cache.TryGet(key, out appId))
                        {
                                error = null;
                                return true;
                        }
                        else if (!new DAL.RpcMerDAL().GetMerAppId(id, out appId))
                        {
                                error = "rpc.mer.appId.get.error";
                                return false;
                        }
                        else
                        {
                                error = null;
                                RpcService.Cache.Set(key, appId);
                                return true;
                        }
                }
                public static bool CheckConAccredit(string remoteIp, string appId, out string error)
                {
                        if (string.IsNullOrEmpty(appId))
                        {
                                error = "rpc.appid.null";
                                return false;
                        }
                        else if (!GetOAuthMer(appId, out RpcMerController mer))
                        {
                                error = mer.Error;
                                return false;
                        }
                        else if (mer.AllowServerIp.Length == 1 && mer.AllowServerIp[0] == "*")
                        {
                                error = null;
                                return true;
                        }
                        else if (mer.AllowServerIp.FindIndex(a => a == remoteIp) != -1)
                        {
                                error = null;
                                return true;
                        }
                        else
                        {
                                error = "rpc.ip.no.accredit";
                                return false;
                        }
                }

        }
}
