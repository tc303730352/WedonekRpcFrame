using RpcCentral.Collect;
using RpcCentral.Collect.Model;
using RpcCentral.Service.Interface;
using WeDonekRpc.Model.Model;
using WeDonekRpc.Model.Server;

namespace RpcCentral.Service.Service
{
    internal class ServerLimitService : IServerLimitService
    {
        private IServerDictateLimitCollect _DictateLimit;
        private IServerLimitConfigCollect _limitConfig;
        private IRpcTokenCollect _Token;
        public ServerLimitService(IServerDictateLimitCollect dictateLimit,
            IServerLimitConfigCollect limitConfig,
            IRpcTokenCollect token)
        {
            this._DictateLimit = dictateLimit;
            this._limitConfig = limitConfig;
            this._Token = token;
        }

        public LimitConfigRes GetServerLimit(GetServerLimit obj)
        {
            RpcTokenCache token = _Token.Get(obj.AccessToken);
            ServerLimitConfig config = _limitConfig.GetServerLimit(token.ConServerId);
            ServerDictateLimit[] limit = _DictateLimit.GetDictateLimit(token.ConServerId);
            return new LimitConfigRes
            {
                LimitConfig = config,
                DictateLimit = limit
            };
        }
    }
}
