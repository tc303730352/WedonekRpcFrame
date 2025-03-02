using RpcSync.DAL;
using RpcSync.Model.DB;
using WeDonekRpc.CacheClient.Interface;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;
using WeDonekRpc.ModularModel.Resource;

namespace RpcSync.Collect.Collect
{
    internal class ResourceModularCollect : IResourceModularCollect
    {
        private readonly IResourceModularDAL _ModularDAL;
        private readonly ICacheController _Cache;
        public ResourceModularCollect (IResourceModularDAL modularDAL, ICacheController cache)
        {
            this._Cache = cache;
            this._ModularDAL = modularDAL;
        }

        public long GetModular (string name, ResourceType type, MsgSource source)
        {
            string key = string.Join("_", name, (int)type, source.SystemType, source.RpcMerId).GetMd5();
            long id = this._FindModularId(key);
            if (id == 0)
            {
                return this._AddModular(name, type, source, key);
            }
            return id;
        }
        private long _FindModularId (string key)
        {
            string cache = string.Concat("ResourceM_", key);
            if (this._Cache.TryGet(cache, out long id))
            {
                return id;
            }
            id = this._ModularDAL.FindModular(key);
            if (id != 0)
            {
                _ = this._Cache.Add(cache, id, new TimeSpan(10, 0, 0, 0));
            }
            return id;
        }
        private long _AddModular (string name, ResourceType type, MsgSource source, string key)
        {
            long id = this._ModularDAL.AddModular(new ResourceModularModel
            {
                ModularKey = key,
                ModularName = name,
                ResourceType = type,
                SystemType = source.SystemType,
                RpcMerId = source.RpcMerId,
                AddTime = DateTime.Now,
                Remark = string.Empty
            });
            string cache = string.Concat("ResourceM_", key);
            _ = this._Cache.Add(cache, id, new TimeSpan(10, 0, 0, 0));
            return id;
        }
    }
}
