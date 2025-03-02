using RpcSync.Service.Interface;
using RpcSync.Service.Model;
using WeDonekRpc.CacheClient.Interface;
using WeDonekRpc.Client.Attr;

namespace RpcSync.Service.Accredit
{
    public class AccreditKeyCache
    {
        public string accreditId;
        public int stateVer;
    }
    [ClassLifetimeAttr(ClassLifetimeType.SingleInstance)]
    internal class AccreditKeySevice : IAccreditKeyCollect
    {
        private static readonly string _AccreditKey = "AccreditKey_";
        private readonly IRedisController _Cache;

        public AccreditKeySevice (IRedisController redis)
        {
            this._Cache = redis;
        }
        public void Renewal (IAccreditToken token, TimeSpan time)
        {
            AccreditKeyCache cache = new AccreditKeyCache
            {
                accreditId = token.AccreditId,
                stateVer = token.Token.StateVer
            };
            string name = string.Concat(_AccreditKey, token.CheckKey);
            if (this._Cache.TryUpdate<AccreditKeyCache>(name, (a) =>
            {
                return a.accreditId == token.AccreditId && a.stateVer >= cache.stateVer ? a : cache;
            }, out AccreditKeyCache key, time))
            {
                if (key.accreditId != token.AccreditId)
                {
                    _ = token.Cancel();
                }
                else
                {
                    this._Cache.SetExpire(name, time);
                }
            }
        }
        public bool TryRemove (string checkKey, string accreditId)
        {
            string name = string.Concat(_AccreditKey, checkKey);
            return this._Cache.TryRemove<AccreditKeyCache>(name, (a) => a.accreditId == accreditId);
        }
        public string KickOut (string checkKey)
        {
            string name = string.Concat(_AccreditKey, checkKey);
            if (this._Cache.TryRemove(name, out AccreditKeyCache cache))
            {
                return cache.accreditId;
            }
            return null;
        }
        public string Set (ref SyncAccredit sync, TimeSpan time)
        {
            string name = string.Concat(_AccreditKey, sync.ApplyKey);
            try
            {
                if (!this._Cache.TryGet(name, out AccreditKeyCache cache) || cache.accreditId != sync.AccreditId)
                {
                    if (this._Cache.Set(name, new AccreditKeyCache
                    {
                        stateVer = sync.StateVer,
                        accreditId = sync.AccreditId
                    }, time))
                    {
                        return cache?.accreditId;
                    }
                }
                return sync.AccreditId;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool Check (string checkKey, string accreditId, out int stateVer)
        {
            string key = string.Concat(_AccreditKey, checkKey);
            if (!this._Cache.TryGet<AccreditKeyCache>(key, out AccreditKeyCache val))
            {
                stateVer = 0;
                return false;
            }
            stateVer = val.stateVer;
            return val.accreditId == accreditId;
        }

    }
}
