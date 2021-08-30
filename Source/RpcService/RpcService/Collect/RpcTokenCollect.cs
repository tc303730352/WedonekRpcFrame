using System;
using System.Collections.Concurrent;
using System.Linq;

using RpcService.Controller;
using RpcService.Model;

using RpcHelper;
using RpcHelper.TaskTools;

namespace RpcService.Collect
{
        internal class RpcTokenCollect
        {
                static RpcTokenCollect()
                {
                        TaskManage.AddTask(new TaskHelper("刷新授权Token!", new TimeSpan(0, 2, 0), new Action(_RefreshToken)));
                }
                private static void _RefreshToken()
                {
                        int time = HeartbeatTimeHelper.HeartbeatTime;
                        string[] keys = _OAuthTokenList.Where(a => a.Value.EffectiveTime <= time).Select(a => a.Key).ToArray();
                        if (keys != null && keys.Length > 0)
                        {
                                keys.ForEach(a => { _OAuthTokenList.TryRemove(a, out RpcToken token); });
                        }
                }
                private static readonly ConcurrentDictionary<string, RpcToken> _OAuthTokenList = new ConcurrentDictionary<string, RpcToken>();

                public static bool ApplyOAuthToken(RpcMerController mer, long serviceId, out string tokenId, out string error)
                {
                        RpcToken token = new RpcToken
                        {
                                AppId = mer.AppId,
                                OAuthMerId = mer.OAuthMerId,
                                ConServerId = serviceId,
                                EffectiveTime = HeartbeatTimeHelper.HeartbeatTime + 7200 * 12,
                                TokenId = Guid.NewGuid().ToString("N")
                        };
                        if (_OAuthTokenList.TryAdd(token.TokenId, token))
                        {
                                token.Save();
                                error = null;
                                tokenId = token.TokenId;
                                return true;
                        }
                        tokenId = null;
                        error = "rpc.token.apply.error";
                        return false;
                }


                public static bool GetOAuthToken(string tokenId, out RpcToken token, out string error)
                {
                        if (_OAuthTokenList.TryGetValue(tokenId, out token))
                        {
                                error = null;
                                return true;
                        }
                        else if (!RpcToken.Load(tokenId, out token))
                        {
                                error = "rpc.token.not.find";
                                return false;
                        }
                        else
                        {
                                error = null;
                                _OAuthTokenList.TryAdd(tokenId, token);
                                return true;
                        }
                }
        }
}
