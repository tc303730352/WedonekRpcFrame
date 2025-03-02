using System;
using WeDonekRpc.CacheClient.Interface;
using WeDonekRpc.Client.Attr;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Helper;
using WeDonekRpc.HttpApiGateway.Idempotent;
using WeDonekRpc.HttpApiGateway.Interface;
namespace WeDonekRpc.HttpApiGateway.Idempotent.Token
{
    [IocName("Memcached")]
    [ClassLifetimeAttr(ClassLifetimeType.SingleInstance)]
    internal class MemcachedTokenService : ITokenIdempotent
    {
        private readonly IIdentityService _Service;
        private readonly IMemcachedController _Memcached;
        private readonly IIdempotentConfig _Config;
        public MemcachedTokenService ( IIdentityService service, IMemcachedController memcached, IIdempotentConfig config )
        {
            this._Memcached = memcached;
            this._Config = config;
            this._Service = service;
        }
        public StatusSaveType SaveType => StatusSaveType.Memcached;
        public string ApplyToken ()
        {
            long token = this._Service.CreateId();
            return !this._Memcached.Add("Idempotent_" + token, token, new TimeSpan(0, 0, this._Config.Expire))
                ? throw new ErrorException("gateway.idempotent.token.apply.fail")
                : token.ToString();
        }

        public void Dispose ()
        {

        }

        public bool SubmitToken ( string tokenId )
        {
            string key = "Idempotent_" + tokenId;
            return this._Memcached.Remove(key);
        }
    }
}
