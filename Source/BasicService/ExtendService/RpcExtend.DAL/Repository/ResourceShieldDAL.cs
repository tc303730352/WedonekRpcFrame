using RpcExtend.Model;
using RpcExtend.Model.DB;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;
using SqlSugar;
using WeDonekRpc.SqlSugar;

namespace RpcExtend.DAL.Repository
{
    internal class ResourceShieldDAL : IResourceShieldDAL
    {
        private readonly IRepository<ResourceShieldModel> _BasicDAL;
        public ResourceShieldDAL (IRepository<ResourceShieldModel> dal)
        {
            this._BasicDAL = dal;
        }

        public ResourceShield Find (string[] keys, string path, ShieldType shieldType)
        {
            long now = DateTime.Now.ToLong();
            ISugarQueryable<ResourceShield>[] list = keys.ConvertAll(c =>
            {
                return this._BasicDAL.Queryable.Where(a => a.ShieIdKey == c && a.ShieldType == shieldType && a.ResourcePath == path && ( a.BeOverdueTime == 0 || a.BeOverdueTime > now ))
                .Select(a => new ResourceShield
                {
                    BeOverdueTime = a.BeOverdueTime,
                    SortNum = a.SortNum,
                    Id = a.Id
                });
            });
            return this._BasicDAL.Get(list, "SortNum desc");
        }

    }
}
