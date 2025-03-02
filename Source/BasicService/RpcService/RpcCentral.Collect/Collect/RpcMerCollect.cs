using WeDonekRpc.CacheClient.Interface;
using RpcCentral.DAL;
using RpcCentral.Model.DB;
using WeDonekRpc.Helper;

namespace RpcCentral.Collect.Collect
{
    internal class RpcMerCollect : IRpcMerCollect
    {
        private readonly IRpcMerDAL _RpcMer;
        private readonly ICacheController _Cache;

        public RpcMerCollect (IRpcMerDAL rpcMer, ICacheController cache)
        {
            this._RpcMer = rpcMer;
            this._Cache = cache;
        }

        public bool GetMer (string appid, out RpcMer mer, out string error)
        {
            string key = string.Concat("RpcMer_", appid);
            if (this._Cache.TryGet(key, out mer))
            {
                error = null;
                return true;
            }
            mer = this._RpcMer.GetRpcMer(appid);
            if (mer == null)
            {
                error = "rpc.store.mer.not.find";
                return false;
            }
            error = null;
            _ = this._Cache.Set(key, mer, new TimeSpan(0, Tools.GetRandom(10, 60), 0));
            return true;
        }
        public RpcMer GetMer (long id)
        {
            string appId = this._GetMerAppId(id);
            if (this.GetMer(appId, out RpcMer mer, out string error))
            {
                return mer;
            }
            throw new ErrorException(error);
        }
        private string _GetMerAppId (long id)
        {
            string key = string.Concat("RpcMer_", id);
            if (this._Cache.TryGet(key, out string appId))
            {
                return appId;
            }
            appId = this._RpcMer.GetMerAppId(id);
            _ = this._Cache.Set(key, appId);
            return appId;
        }
        public bool CheckConAccredit (string remoteIp, string appId, out string error)
        {
            if (string.IsNullOrEmpty(appId))
            {
                error = "rpc.appid.null";
                return false;
            }
            else if (!this.GetMer(appId, out RpcMer mer, out error))
            {
                return false;
            }
            else if (mer.AllowServerIp.Length == 1 && mer.AllowServerIp[0] == "*")
            {
                error = null;
                return true;
            }
            else if (mer.AllowServerIp.FindIndex(a => a == remoteIp) != -1)
            {
                error = null;
                return true;
            }
            else
            {
                error = "rpc.ip.no.accredit";
                return false;
            }
        }

        public void Refresh (long merId)
        {
            string appId = this._GetMerAppId(merId);
            if (!appId.IsNull())
            {
                string key = string.Concat("RpcMer_", appId);
                _ = this._Cache.Remove(key);
            }
        }
    }
}
