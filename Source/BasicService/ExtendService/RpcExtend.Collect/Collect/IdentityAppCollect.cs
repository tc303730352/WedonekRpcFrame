using WeDonekRpc.CacheClient.Interface;
using RpcExtend.DAL;
using RpcExtend.Model;

namespace RpcExtend.Collect.Collect
{
    internal class IdentityAppCollect : IIdentityAppCollect
    {
        private readonly IIdentityAppDAL _IdentityApp;
        private readonly ICacheController _Cache;

        public IdentityAppCollect (IIdentityAppDAL identityApp, ICacheController cache)
        {
            this._IdentityApp = identityApp;
            this._Cache = cache;
        }
        public IdentityApp GetByAppId (string appId)
        {
            string key = string.Concat("Identity_", appId);
            if (this._Cache.TryGet(key, out IdentityApp app))
            {
                return app;
            }
            app = this._IdentityApp.GetByAppId(appId);
            if (app == null)
            {
                app = new IdentityApp
                {
                    IsEnable = false
                };
            }
            _ = this._Cache.Set(key, app);
            return app;
        }
        public void Refresh (string appId)
        {
            string key = string.Concat("Identity_", appId);
            _ = this._Cache.Remove(key);
        }
    }
}
