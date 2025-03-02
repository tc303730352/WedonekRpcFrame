using WeDonekRpc.CacheClient.Interface;
using RpcSync.Model;

namespace RpcSync.Service.Accredit
{
    internal class DbCacheAccredit
    {
        private string _CacheKey;
        private readonly ICacheController _Cache;
        public string AccreditId { get; private set; }

        public DbCacheAccredit(ICacheController cache)
        {
            _Cache = cache;
        }

        protected void InitAccreditId(string accreditId)
        {
            this.AccreditId = accreditId;
            this._CacheKey = "AccreditCache_" + accreditId;
        }
        protected void SetCache(AccreditTokenDatum token)
        {
            DateTime time = token.OverTime;
            if (time < DateTime.Now)
            {
                time = DateTime.Now.AddMinutes(1);
            }
            _Cache.Set(this._CacheKey, token, time);
        }
        protected void RmoveCache()
        {
            _Cache.Remove(_CacheKey);
        }
        protected bool TryGet(out AccreditTokenDatum token)
        {
            return _Cache.TryGet(this._CacheKey, out token);
        }
    }
}
