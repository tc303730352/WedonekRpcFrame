using WeDonekRpc.Helper;
using WeDonekRpc.Helper.IdGenerator;
using WeDonekRpc.Model;
using RpcStore.Model.DB;
using WeDonekRpc.SqlSugar;
namespace RpcStore.DAL.Repository
{
    internal class ServerGroupDAL : IServerGroupDAL
    {
        private readonly IRepository<ServerGroupModel> _BasicDAL;
        public ServerGroupDAL (IRepository<ServerGroupModel> dal)
        {
            this._BasicDAL = dal;
        }

        public bool CheckIsRepeat (string typeVal)
        {
            return this._BasicDAL.IsExist(c => c.TypeVal == typeVal);
        }
        public long Add (ServerGroupModel datum)
        {
            datum.Id = IdentityHelper.CreateId();
            this._BasicDAL.Insert(datum);
            return datum.Id;
        }

        public void Set (long id, string name)
        {
            if (!this._BasicDAL.Update(c => c.GroupName == name, c => c.Id == id))
            {
                throw new ErrorException("rpc.store.server.group.set.error");
            }
        }
        public ServerGroupModel[] Query (string name, IBasicPage paging, out int count)
        {
            paging.InitOrderBy("Id", true);
            if (name.IsNotNull())
            {
                return this._BasicDAL.Query(a => a.GroupName.Contains(name), paging, out count);
            }
            return this._BasicDAL.Query(paging, out count);
        }
        public ServerGroupModel[] Gets ()
        {
            return this._BasicDAL.GetAll();
        }
        public ServerGroupModel Get (long id)
        {
            return this._BasicDAL.Get(c => c.Id == id);
        }
        public Dictionary<long, string> GetGroupName (long[] ids)
        {
            var list = this._BasicDAL.Gets(a => ids.Contains(a.Id), c => new
            {
                c.Id,
                c.GroupName
            });
            return list.ToDictionary(c => c.Id, c => c.GroupName);
        }
        public string GetName (long id)
        {
            return this._BasicDAL.Get(a => a.Id == id, c => c.GroupName);
        }
        public void Delete (long id)
        {
            if (!this._BasicDAL.Delete(a => a.Id == id))
            {
                throw new ErrorException("rpc.store.server.group.delete.error");
            }
        }

        public ServerGroupModel[] Gets (long[] ids)
        {
            return this._BasicDAL.Gets(c => ids.Contains(c.Id));
        }
    }
}
