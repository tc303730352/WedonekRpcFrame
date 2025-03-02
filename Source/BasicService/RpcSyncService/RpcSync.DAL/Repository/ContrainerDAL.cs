using RpcSync.Model.DB;
using WeDonekRpc.SqlSugar;

namespace RpcSync.DAL.Repository
{
    internal class ContrainerDAL : IContrainerDAL
    {
        private readonly IRepository<Contrainer> _BasicDAL;
        public ContrainerDAL (IRepository<Contrainer> dal)
        {
            this._BasicDAL = dal;
        }

        public long[] GetIds (long groupId)
        {
            return this._BasicDAL.Gets(a => a.GroupId == groupId, a => a.Id);
        }
    }
}
