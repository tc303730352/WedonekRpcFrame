using System;
using WeDonekRpc.CacheClient.Interface;
using RpcCentral.DAL;
using WeDonekRpc.Model.Model;

namespace RpcCentral.Collect.Collect
{
    internal class ServerDictateLimitCollect : IServerDictateLimitCollect
    {
        private IServerDictateLimitDAL _BasicDAL;
        private ICacheController _Cache;
        public ServerDictateLimitCollect(IServerDictateLimitDAL dal, ICacheController cache)
        {
            this._Cache = cache;
            this._BasicDAL = dal;
        }
        public void Refresh(long serverId)
        {
            string key = string.Concat("DicateLimit_", serverId);
            this._Cache.Remove(key);

        }
        public ServerDictateLimit[] GetDictateLimit(long serverId)
        {
            string key = string.Concat("DicateLimit_", serverId);
            if (this._Cache.TryGet(key, out ServerDictateLimit[] limits))
            {
                return limits;
            }
            limits = _BasicDAL.GetDictateLimit(serverId);
            this._Cache.Add(key, limits, new TimeSpan(10, 0, 0, 0));
            return limits;
        }
    }
}
