
using WeDonekRpc.Client;
using WeDonekRpc.Helper;

using WeDonekRpc.Model;
using RpcStore.DAL;
using RpcStore.Model.DB;
using RpcStore.RemoteModel.ServerGroup.Model;
using WeDonekRpc.SqlSugar;

namespace RpcStore.Collect.lmpl
{
    internal class ServerGroupCollect : IServerGroupCollect
    {
        private readonly IServerGroupDAL _BasicDAL;
        private readonly IRemoteServerTypeDAL _ServiceType;
        private readonly ITransactionService _Tran;
        public ServerGroupCollect (IServerGroupDAL basicDAL,
            ITransactionService tran,
            IRemoteServerTypeDAL serviceType)
        {
            this._Tran = tran;
            this._BasicDAL = basicDAL;
            this._ServiceType = serviceType;
        }

        public void CheckIsRepeat (string typeVal)
        {
            if (this._BasicDAL.CheckIsRepeat(typeVal))
            {
                throw new ErrorException("rpc.store.server.group.type.repeat");
            }
        }
        public long AddGroup (ServerGroupAdd group)
        {
            this.CheckIsRepeat(group.TypeVal);
            ServerGroupModel add = group.ConvertMap<ServerGroupAdd, ServerGroupModel>();
            return this._BasicDAL.Add(add);
        }

        public void SetGroup (ServerGroupModel group, string name)
        {
            if (group.GroupName == name)
            {
                return;
            }
            this._BasicDAL.Set(group.Id, name);
        }
        public ServerGroupModel[] QueryGroup (string name, IBasicPage paging, out int count)
        {
            return this._BasicDAL.Query(name, paging, out count);
        }
        public ServerGroupModel[] GetGroups ()
        {
            return this._BasicDAL.Gets();
        }
        public ServerGroupModel GetGroup (long id)
        {
            ServerGroupModel group = this._BasicDAL.Get(id);
            if (group == null)
            {
                throw new ErrorException("rpc.store.server.group.not.find");
            }
            return group;
        }

        public void Delete (ServerGroupModel group)
        {
            long[] ids = this._ServiceType.GetIds(group.Id);
            if (ids.IsNull())
            {
                this._BasicDAL.Delete(group.Id);
                return;
            }
            using (ILocalTransaction tran = this._Tran.ApplyTran())
            {
                this._ServiceType.Delete(ids);
                this._BasicDAL.Delete(group.Id);
                tran.Commit();
            }
        }
        public Dictionary<long, string> GetGroupName (long[] ids)
        {
            if (ids.IsNull())
            {
                return [];
            }
            return this._BasicDAL.GetGroupName(ids);
        }
        public string GetName (long id)
        {
            return this._BasicDAL.GetName(id);
        }
        public ServerGroupModel[] GetGroup (long[] ids)
        {
            return this._BasicDAL.Gets(ids);
        }
    }
}
