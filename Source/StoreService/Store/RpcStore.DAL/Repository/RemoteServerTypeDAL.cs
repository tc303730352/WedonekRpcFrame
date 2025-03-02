using WeDonekRpc.Helper;
using WeDonekRpc.Helper.IdGenerator;
using WeDonekRpc.Model;
using RpcStore.Model.DB;
using RpcStore.RemoteModel.ServerType.Model;
using SqlSugar;
using WeDonekRpc.SqlSugar;

namespace RpcStore.DAL.Repository
{
    internal class RemoteServerTypeDAL : IRemoteServerTypeDAL
    {
        private readonly IRepository<RemoteServerTypeModel> _BasicDAL;
        public RemoteServerTypeDAL (IRepository<RemoteServerTypeModel> dal)
        {
            this._BasicDAL = dal;
        }
        public long Add (RemoteServerTypeModel add)
        {
            add.AddTime = DateTime.Now;
            add.Id = IdentityHelper.CreateId();
            this._BasicDAL.Insert(add);
            return add.Id;
        }
        public bool CheckIsRepeat (string typeVal)
        {
            return this._BasicDAL.IsExist(c => c.TypeVal == typeVal);
        }

        public bool CheckIsExists (long groupId)
        {
            return this._BasicDAL.IsExist(c => c.GroupId == groupId);
        }

        public void Set (long id, ServerTypeSet param)
        {
            if (!this._BasicDAL.Update(param, c => c.Id == id))
            {
                throw new ErrorException("rpc.store.server.type.set.error");
            }
        }

        public void Delete (long id)
        {
            if (!this._BasicDAL.Delete(c => c.Id == id))
            {
                throw new ErrorException("rpc.store.server.type.delete.error");
            }
        }
        public long GetGroupId (long id)
        {
            return this._BasicDAL.Get(a => a.Id == id, a => a.GroupId);
        }
        public RemoteServerTypeModel Get (long id)
        {
            return this._BasicDAL.Get(c => c.Id == id);
        }
        public ServerType[] GetServiceTypes (long groupId)
        {
            return this._BasicDAL.Gets<ServerType>(c => c.GroupId == groupId);
        }
        public string GetTypeVal (long id)
        {
            return this._BasicDAL.Get(c => c.Id == id, c => c.TypeVal);
        }
        public ServerType[] Query (ServerTypeQuery query, IBasicPage paging, out int count)
        {
            return this._BasicDAL.Query<ServerType>(query.ToWhere(), paging, out count);
        }
        public string GetName (string typeVal)
        {
            return this._BasicDAL.Get(a => a.TypeVal == typeVal, c => c.SystemName);
        }
        public string GetName (long id)
        {
            return this._BasicDAL.Get(a => a.Id == id, c => c.SystemName);
        }
        public Dictionary<string, string> GetNames (string[] types)
        {
            var list = this._BasicDAL.Gets(c => types.Contains(c.TypeVal), c => new
            {
                c.TypeVal,
                c.SystemName
            });
            return list.ToDictionary(c => c.TypeVal, c => c.SystemName);
        }
        public Dictionary<long, string> GetNames (long[] ids)
        {
            var list = this._BasicDAL.Gets(c => ids.Contains(c.Id), c => new
            {
                c.Id,
                c.SystemName
            });
            return list.ToDictionary(c => c.Id, c => c.SystemName);
        }
        public BasicServerType[] GetBasic (long[] ids)
        {
            return this._BasicDAL.Gets<BasicServerType>(c => ids.Contains(c.Id));
        }

        public ServerType[] Gets (long[] ids)
        {
            return this._BasicDAL.Gets<ServerType>(c => ids.Contains(c.Id));
        }
        public ServerType[] Gets (string[] types)
        {
            return this._BasicDAL.Gets<ServerType>(c => types.Contains(c.TypeVal));
        }
        public long[] GetIds (long groupId)
        {
            return this._BasicDAL.Gets(c => c.GroupId == groupId, c => c.Id);
        }
        public BasicServerType[] GetBasic (string[] types)
        {
            return this._BasicDAL.Gets<BasicServerType>(c => types.Contains(c.TypeVal));
        }
        public void Delete (long[] ids)
        {
            if (!this._BasicDAL.Delete(c => ids.Contains(c.Id)))
            {
                throw new ErrorException("rpc.store.server.type.delete.error");
            }
        }

        public ServerType[] GetAll ()
        {
            return this._BasicDAL.GetAll<ServerType>();
        }

        public Dictionary<long, string> GetSystemTypeVal (long[] typeId)
        {
            return this._BasicDAL.Gets(a => typeId.Contains(a.Id), a => new
            {
                a.Id,
                a.TypeVal
            }).ToDictionary(a => a.Id, a => a.TypeVal);
        }

        public long GetIdByTypeVal (string typeVal)
        {
            return this._BasicDAL.Get(c => c.TypeVal == typeVal, c => c.Id);
        }

        public Dictionary<long, int> GetNum (long[] groupId)
        {
            return this._BasicDAL.GroupBy(a => groupId.Contains(a.GroupId), a => a.GroupId, a => new
            {
                a.GroupId,
                count = SqlFunc.AggregateCount(a.GroupId)
            }).ToDictionary(a => a.GroupId, a => a.count);
        }
    }
}
