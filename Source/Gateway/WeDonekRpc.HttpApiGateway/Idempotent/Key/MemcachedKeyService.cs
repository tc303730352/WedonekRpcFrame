using System;
using WeDonekRpc.CacheClient.Interface;
using WeDonekRpc.Client.Attr;
using WeDonekRpc.HttpApiGateway.Interface;

namespace WeDonekRpc.HttpApiGateway.Idempotent.Key
{
    [IocName("Memcached")]
    [ClassLifetimeAttr(ClassLifetimeType.SingleInstance)]
    internal class MemcachedKeyService : IKeyIdempotent
    {
        private readonly IMemcachedController _Memcached;
        private readonly IIdempotentConfig _Config;
        public MemcachedKeyService ( IMemcachedController memcached, IIdempotentConfig config )
        {
            this._Memcached = memcached;
            this._Config = config;
        }
        public StatusSaveType SaveType => StatusSaveType.Memcached;


        public void Dispose ()
        {

        }

        public bool SubmitToken ( string tokenId )
        {
            string key = "Idempotent_" + tokenId;
            return !this._Memcached.Add(key, tokenId, new TimeSpan(0, 0, this._Config.Expire));
        }
    }
}
