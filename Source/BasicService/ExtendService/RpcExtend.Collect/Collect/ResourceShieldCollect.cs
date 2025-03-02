using RpcExtend.Collect.Model;
using RpcExtend.DAL;
using RpcExtend.Model;
using WeDonekRpc.CacheClient.Interface;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;
using WeDonekRpc.ModularModel.Shield.Msg;

namespace RpcExtend.Collect.Collect
{
    internal class ResourceShieldCollect : IResourceShieldCollect
    {
        private readonly IResourceShieldDAL _BasicDAL;
        private readonly ICacheController _Cache;
        public ResourceShieldCollect (IResourceShieldDAL basicDAL, ICacheController cache)
        {
            this._BasicDAL = basicDAL;
            this._Cache = cache;
        }

        public ResourceShield GetShield (ShieIdQuery query, string path)
        {
            string key = "ShieId_" + string.Join('_', query.RpcMerId, (byte)query.ShieIdType, query.SystemType, query.ServerId, query.VerNum, path).GetMd5();
            if (this._Cache.TryGet(key, out ResourceShield shield))
            {
                return shield;
            }
            shield = this._GetShield(query, path);
            if (shield == null)
            {
                shield = new ResourceShield();
            }
            _ = this._Cache.Set(key, shield);
            return shield;
        }
        public void Refresh (string path, ShieIdQuery query)
        {
            string key = "ShieId_" + string.Join('_', query.RpcMerId, (byte)query.ShieIdType, query.SystemType, query.ServerId, query.VerNum, path).GetMd5();
            _ = this._Cache.Remove(key);
            if (query.ShieIdType == ShieldType.接口)
            {
                new RefreshApiShieId
                {
                    Path = path
                }.Send(new string[]
                {
                query.SystemType
                }, query.RpcMerId);
            }
            else
            {
                new RefreshRpcShieId
                {
                    SysDictate = path
                }.Send(new string[]
            {
                query.SystemType
            }, query.RpcMerId);
            }
        }
        private ResourceShield _GetShield (ShieIdQuery query, string path)
        {
            string[] keys = new string[]
            {
                string.Join('_',query.RpcMerId,query.SystemType,query.ServerId,query.VerNum),//4
                string.Join('_',query.RpcMerId,query.SystemType,query.ServerId,"0"),//2
                string.Join('_',query.RpcMerId,query.SystemType,"0","0"),//1
                string.Join('_',query.RpcMerId,query.SystemType,"0",query.VerNum)//3
            };
            return this._BasicDAL.Find(keys, path, query.ShieIdType);
        }

    }
}
