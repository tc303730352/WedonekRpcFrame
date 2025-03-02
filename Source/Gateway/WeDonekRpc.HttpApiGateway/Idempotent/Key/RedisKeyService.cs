using System;
using WeDonekRpc.CacheClient.Interface;
using WeDonekRpc.Client.Attr;
using WeDonekRpc.HttpApiGateway.Interface;

namespace WeDonekRpc.HttpApiGateway.Idempotent.Key
{
    [IocName("Redis")]
    [ClassLifetimeAttr(ClassLifetimeType.SingleInstance)]
    internal class RedisKeyService : IKeyIdempotent
    {
        private readonly IRedisController _Redis;
        private readonly IIdempotentConfig _Config;
        public RedisKeyService ( IRedisController redis, IIdempotentConfig config )
        {
            this._Redis = redis;
            this._Config = config;
        }
        public StatusSaveType SaveType => StatusSaveType.Redis;
        public void Dispose ()
        {

        }
        public bool SubmitToken ( string tokenId )
        {
            string key = "Idempotent_" + tokenId;
            return this._Redis.Add(key, tokenId, new TimeSpan(0, 0, this._Config.Expire));
        }
    }
}
