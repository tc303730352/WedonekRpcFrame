using WeDonekRpc.Client;
using WeDonekRpc.Helper;

using WeDonekRpc.Model;
using RpcStore.DAL;
using RpcStore.Model.DB;
using RpcStore.RemoteModel.ServerType.Model;

namespace RpcStore.Collect.lmpl
{
    internal class ServerTypeCollect : IServerTypeCollect
    {
        private readonly IRemoteServerTypeDAL _BasicDAL;

        public ServerTypeCollect (IRemoteServerTypeDAL basicDAL)
        {
            this._BasicDAL = basicDAL;
        }

        public long Add (ServerTypeAdd datum)
        {
            this.CheckIsRepeat(datum.TypeVal);
            RemoteServerTypeModel add = datum.ConvertMap<ServerTypeAdd, RemoteServerTypeModel>();
            return this._BasicDAL.Add(add);
        }
        public void CheckIsRepeat (string typeVal)
        {
            if (this._BasicDAL.CheckIsRepeat(typeVal))
            {
                throw new ErrorException("rpc.store.server.type.repeat");
            }
        }

        public ServerType[] Gets (long groupId)
        {
            return this._BasicDAL.GetServiceTypes(groupId);
        }

        public bool Set (RemoteServerTypeModel type, ServerTypeSet param)
        {
            if (param.IsEquals(type))
            {
                return false;
            }
            this._BasicDAL.Set(type.Id, param);
            return true;
        }

        public void Delete (RemoteServerTypeModel type)
        {
            this._BasicDAL.Delete(type.Id);
        }
        public long GetGroupId (long id)
        {
            long groupId = this._BasicDAL.GetGroupId(id);
            if (groupId == 0)
            {
                throw new ErrorException("rpc.store.server.type.not.find");
            }
            return groupId;
        }
        public RemoteServerTypeModel Get (long id)
        {
            RemoteServerTypeModel type = this._BasicDAL.Get(id);
            if (type == null)
            {
                throw new ErrorException("rpc.store.server.type.not.find");
            }
            return type;
        }

        public ServerType[] Query (ServerTypeQuery query, IBasicPage paging, out int count)
        {
            return this._BasicDAL.Query(query, paging, out count);
        }
        public string GetName (string typeVal)
        {
            return this._BasicDAL.GetName(typeVal);
        }
        public string GetName (long id)
        {
            return this._BasicDAL.GetName(id);
        }
        public string GetTypeVal (long id)
        {
            return this._BasicDAL.GetTypeVal(id);
        }
        public ServerType[] Gets (long[] ids)
        {
            if (ids.Length == 0)
            {
                return Array.Empty<ServerType>();
            }
            return this._BasicDAL.Gets(ids);
        }

        public BasicServerType[] GetBasic (long[] ids)
        {
            return this._BasicDAL.GetBasic(ids);
        }
        public Dictionary<long, string> GetNames (long[] ids)
        {
            return this._BasicDAL.GetNames(ids);
        }
        public Dictionary<string, string> GetNames (string[] types)
        {
            if (types.IsNull())
            {
                return null;
            }
            return this._BasicDAL.GetNames(types);
        }
        public BasicServerType[] GetBasic (string[] types)
        {
            return this._BasicDAL.GetBasic(types);
        }

        public ServerType[] Gets (string[] types)
        {
            return this._BasicDAL.Gets(types);
        }

        public bool CheckIsExists (long groupId)
        {
            return this._BasicDAL.CheckIsExists(groupId);
        }

        public ServerType[] GetAll ()
        {
            return this._BasicDAL.GetAll();
        }

        public Dictionary<long, string> GetSystemTypeVal (long[] typeId)
        {
            return this._BasicDAL.GetSystemTypeVal(typeId);
        }

        public long GetIdByTypeVal (string typeVal)
        {
            return this._BasicDAL.GetIdByTypeVal(typeVal);
        }

        public Dictionary<long, int> GetNum(long[] groupId)
        {
            return this._BasicDAL.GetNum(groupId);
        }
    }
}
