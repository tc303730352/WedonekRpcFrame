using WeDonekRpc.Helper;
using RpcStore.DAL.Model;
using RpcStore.Model.DB;
using WeDonekRpc.SqlSugar;

namespace RpcStore.DAL.Repository
{
    /// <summary>
    /// 节点版本管理
    /// </summary>
    internal class ServiceVerConfigDAL : IServiceVerConfigDAL
    {
        private readonly IRepository<ServiceVerConfigModel> _BasicDAL;
        public ServiceVerConfigDAL (IRepository<ServiceVerConfigModel> dal)
        {
            this._BasicDAL = dal;
        }

        public ServiceVerConfigModel[] Gets (long schemeId)
        {
            return this._BasicDAL.Gets(c => c.SchemeId == schemeId);
        }
        public SchemeSystemType[] GetSchemeType (Dictionary<long, int> systemType)
        {
            SqlSugar.ISugarQueryable<SchemeSystemType>[] list = systemType.ConvertAll(c => this._BasicDAL.Queryable.Where(a => a.SystemTypeId == c.Key && a.VerNum == c.Value).Select(a => new SchemeSystemType
            {
                SchemeId = a.SchemeId,
                SystemTypeId = a.SystemTypeId
            }));
            return this._BasicDAL.Gets(list);
        }
    }
}
