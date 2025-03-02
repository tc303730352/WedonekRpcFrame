using WeDonekRpc.Helper;
using WeDonekRpc.Helper.IdGenerator;
using WeDonekRpc.Model;
using RpcStore.Model.DB;
using RpcStore.RemoteModel.DictateLimit.Model;
using WeDonekRpc.SqlSugar;

namespace RpcStore.DAL.Repository
{
    internal class ServerDictateLimitDAL : IServerDictateLimitDAL
    {
        private readonly IRepository<ServerDictateLimitModel> _BasicDAL;
        public ServerDictateLimitDAL (IRepository<ServerDictateLimitModel> dal)
        {
            this._BasicDAL = dal;
        }
        public long Add (ServerDictateLimitModel add)
        {
            add.Id = IdentityHelper.CreateId();
            this._BasicDAL.Insert(add);
            return add.Id;
        }

        public ServerDictateLimitModel Get (long id)
        {
            return this._BasicDAL.Get(c => c.Id == id);
        }
        public void Delete (long id)
        {
            if (!this._BasicDAL.Delete(a => a.Id == id))
            {
                throw new ErrorException("rpc.store.dictate.delete.error");
            }
        }
        public void Set (long id, DictateLimitSet limit)
        {
            if (!this._BasicDAL.Update(limit, a => a.Id == id))
            {
                throw new ErrorException("rpc.store.dictate.set.error");
            }
        }
        public void Clear (long serverId)
        {
            _ = this._BasicDAL.Delete(c => c.ServerId == serverId);
        }
        public ServerDictateLimitModel[] Query (DictateLimitQuery query, IBasicPage paging, out int count)
        {
            paging.InitOrderBy("Id", true);
            return this._BasicDAL.Query(query.ToWhere(), paging, out count);
        }

        public bool CheckIsExists (long serverId, string dictate)
        {
            return this._BasicDAL.IsExist(c => c.ServerId == serverId && c.Dictate == dictate);
        }
    }
}
