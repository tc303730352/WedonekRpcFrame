using WeDonekRpc.CacheClient.Interface;
using RpcCentral.Collect.Model;
using RpcCentral.Common;
using RpcCentral.Model.DB;
using WeDonekRpc.Helper;
using System.Collections.Concurrent;

namespace RpcCentral.Collect.Collect
{
    internal class RpcTokenCollect : IRpcTokenCollect
    {
        private static readonly ConcurrentDictionary<string, RpcTokenCache> _OAuthTokenList = new ConcurrentDictionary<string, RpcTokenCache>();
        private readonly IRedisController _Cache;
        private static readonly Timer _RefreshTimer;
        public RpcTokenCollect()
        {
            if (!RpcContralConfig.TokenIsLocalSave)
            {
                this._Cache = UnityHelper.Resolve<IRedisController>();
            }
        }
        static RpcTokenCollect()
        {
            _RefreshTimer = new Timer(_RefreshToken, null, 6000, 120000);
        }
        private static void _RefreshToken(object state)
        {
            if (_OAuthTokenList.IsEmpty)
            {
                return;
            }
            int time = HeartbeatTimeHelper.HeartbeatTime;
            string[] keys = _OAuthTokenList.Where(a => a.Value.EffectiveTime <= time).Select(a => a.Key).ToArray();
            if (keys != null && keys.Length > 0)
            {
                keys.ForEach(a => _OAuthTokenList.TryRemove(a, out _));
            }
        }

        public string Apply(RpcMer mer, long serviceId)
        {
            RpcTokenCache token = new RpcTokenCache
            {
                AppId = mer.AppId,
                RpcMerId = mer.Id,
                ConServerId = serviceId,
                EffectiveTime = HeartbeatTimeHelper.HeartbeatTime + 7200 * 12,
                TokenId = Guid.NewGuid().ToString("N")
            };
            if (_OAuthTokenList.TryAdd(token.TokenId, token))
            {
                this._Save(token);
                return token.TokenId;
            }
            throw new ErrorException("rpc.token.apply.error");
        }
        private void _Save(RpcTokenCache cache)
        {
            if (RpcContralConfig.TokenIsLocalSave)
            {
                return;
            }
            string key = string.Concat("Token_", cache.TokenId);
            this._Cache.Set(key, this, HeartbeatTimeHelper.GetTime(cache.EffectiveTime - 5));
        }
        private RpcTokenCache _Load(string tokenId)
        {
            if (RpcContralConfig.TokenIsLocalSave)
            {
                return null;
            }
            string key = string.Concat("Token_", tokenId);
            if (this._Cache.TryGet(key, out RpcTokenCache token))
            {
                return token;
            }
            throw new ErrorException("rpc.token.not.find");
        }
        public RpcTokenCache Get(string tokenId)
        {
            if (_OAuthTokenList.TryGetValue(tokenId, out RpcTokenCache token))
            {
                return token;
            }
            token = this._Load(tokenId);
            _OAuthTokenList.TryAdd(tokenId, token);
            return token;
        }

        public void SetConServerId(RpcTokenCache token, long serverId)
        {
            if (token.ConServerId != serverId)
            {
                token.ConServerId = serverId;
                this._Save(token);
            }
        }
    }
}
