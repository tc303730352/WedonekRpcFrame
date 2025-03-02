using System;
using WeDonekRpc.CacheClient.Interface;
using WeDonekRpc.Client.Attr;
using WeDonekRpc.HttpApiGateway.Interface;

namespace WeDonekRpc.HttpApiGateway.Idempotent.Key
{
    [IocName("Cache")]
    [ClassLifetimeAttr(ClassLifetimeType.SingleInstance)]
    internal class CacheKeyService : IKeyIdempotent
    {
        private readonly ICacheController _Cache;
        private readonly IIdempotentConfig _Config;
        public CacheKeyService (ICacheController cache, IIdempotentConfig config)
        {
            this._Cache = cache;
            this._Config = config;
        }
        public StatusSaveType SaveType => StatusSaveType.Cache;
        public void Dispose ()
        {

        }

        public bool SubmitToken (string tokenId)
        {
            string key = "Idem_" + tokenId;
            return this._Cache.Add(key, tokenId, new TimeSpan(0, 0, this._Config.Expire));
        }
    }
}
