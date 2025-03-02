using System;
using WeDonekRpc.CacheClient.Interface;
using WeDonekRpc.Client.Attr;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Helper;
using WeDonekRpc.HttpApiGateway.Interface;

namespace WeDonekRpc.HttpApiGateway.Idempotent.Token
{
    [IocName("Cache")]
    [ClassLifetimeAttr(ClassLifetimeType.SingleInstance)]
    internal class CacheTokenService : ITokenIdempotent
    {
        private readonly IIdentityService _Service;
        private readonly ICacheController _Cache;
        private readonly IIdempotentConfig _Config;
        public CacheTokenService (IIdentityService service, ICacheController cache, IIdempotentConfig config)
        {
            this._Cache = cache;
            this._Config = config;
            this._Service = service;
        }
        public StatusSaveType SaveType => StatusSaveType.Cache;
        public void Dispose ()
        {

        }
        public string ApplyToken ()
        {
            long token = this._Service.CreateId();
            return !this._Cache.Add("Idem_" + token, token, new TimeSpan(0, 0, this._Config.Expire))
                ? throw new ErrorException("gateway.idempotent.token.apply.fail")
                : token.ToString();
        }
        public bool SubmitToken (string tokenId)
        {
            string key = "Idem_" + tokenId;
            return this._Cache.Remove(key);
        }
    }
}
