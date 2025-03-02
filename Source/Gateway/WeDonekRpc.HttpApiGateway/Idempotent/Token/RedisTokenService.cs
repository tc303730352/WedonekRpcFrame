using System;
using WeDonekRpc.CacheClient.Interface;
using WeDonekRpc.Client.Attr;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Helper;
using WeDonekRpc.HttpApiGateway.Interface;

namespace WeDonekRpc.HttpApiGateway.Idempotent.Token
{
    [IocName("Redis")]
    [ClassLifetimeAttr(ClassLifetimeType.SingleInstance)]
    internal class RedisTokenService : ITokenIdempotent
    {
        private readonly IIdentityService _Service;
        private readonly IRedisController _Redis;
        private readonly IIdempotentConfig _Config;
        public RedisTokenService (IIdentityService service, IRedisController redis, IIdempotentConfig config)
        {
            this._Redis = redis;
            this._Config = config;
            this._Service = service;
        }
        public StatusSaveType SaveType => StatusSaveType.Redis;
        public void Dispose ()
        {

        }
        public string ApplyToken ()
        {
            long token = this._Service.CreateId();
            return !this._Redis.Add("Idempotent_" + token, token, new TimeSpan(0, 0, this._Config.Expire))
                ? throw new ErrorException("gateway.idempotent.token.apply.fail")
                : token.ToString();
        }
        public bool SubmitToken (string tokenId)
        {
            string key = "Idempotent_" + tokenId;
            return this._Redis.Remove(key);
        }
    }
}
