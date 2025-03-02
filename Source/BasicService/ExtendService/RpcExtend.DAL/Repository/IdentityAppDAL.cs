using RpcExtend.Model;
using RpcExtend.Model.DB;
using WeDonekRpc.SqlSugar;

namespace RpcExtend.DAL.Repository
{
    internal class IdentityAppDAL : IIdentityAppDAL
    {
        private readonly IRepository<IdentityAppModel> _BasicDAL;
        public IdentityAppDAL (IRepository<IdentityAppModel> dal)
        {
            this._BasicDAL = dal;
        }
        public IdentityApp GetByAppId (string appId)
        {
            return this._BasicDAL.Get<IdentityApp>(c => c.AppId == appId);
        }
    }
}
