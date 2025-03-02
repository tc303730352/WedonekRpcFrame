using System.Data;
using WeDonekRpc.CacheClient.Interface;
using RpcExtend.DAL;
using RpcExtend.Model;
using WeDonekRpc.Helper;

namespace RpcExtend.Collect.Collect
{
    internal class IpBlackListCollect : IIpBlackListCollect
    {
        private readonly IIpBlackListDAL _BasicDAL;
        private readonly ICacheController _Cache;
        public IpBlackListCollect (IIpBlackListDAL basicDAL, ICacheController cache)
        {
            this._BasicDAL = basicDAL;
            this._Cache = cache;
        }

        public void Refresh (long rpcMerId, string sysType)
        {
            string key = string.Join("_", "IpBlack", rpcMerId, sysType);
            _ = this._Cache.Remove(key);
        }
        public long GetMaxVer (long rpcMerId, string sysType)
        {
            string key = string.Join("_", "IpBlack", rpcMerId, sysType);
            if (this._Cache.TryGet(key, out long ver))
            {
                return ver;
            }
            ver = this._BasicDAL.GetMaxVer(rpcMerId, sysType);
            _ = this._Cache.Set(key, ver, new TimeSpan(31, 0, 0, 0));
            return ver;
        }

        public IpBlack[] GetIpBlacks (long rpcMerId, string sysType, long beginVer, long endVer)
        {
            IpBlack[] list = this._BasicDAL.GetIpBlacks(rpcMerId, sysType, beginVer, endVer);
            if (list.IsNull())
            {
                return list;
            }
            return list.OrderByDescending(a => a.Id).Distinct().ToArray();
        }
    }
}
