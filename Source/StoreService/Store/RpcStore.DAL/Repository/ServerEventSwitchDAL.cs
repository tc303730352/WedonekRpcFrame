using RpcStore.Model.ExtendDB;
using RpcStore.Model.ServerEventSwitch;
using RpcStore.RemoteModel.ServerEventSwitch.Model;
using WeDonekRpc.Client;
using WeDonekRpc.Helper.IdGenerator;
using WeDonekRpc.Model;

namespace RpcStore.DAL.Repository
{
    internal class ServerEventSwitchDAL : IServerEventSwitchDAL
    {
        private readonly IRpcExtendResource<ServerEventSwitchModel> _Resource;

        public ServerEventSwitchDAL (IRpcExtendResource<ServerEventSwitchModel> resource)
        {
            this._Resource = resource;
        }

        public ServerEventSwitch[] Query (EventSwitchQuery query, IBasicPage paging, out int count)
        {
            paging.InitOrderBy("Id", true);
            return this._Resource.Query<ServerEventSwitch>(query.ToWhere(), paging, out count);
        }
        public bool CheckIsRepeat (long rpcMerId, long serverId, string eventKey)
        {
            return this._Resource.IsExist(c => c.RpcMerId == rpcMerId && c.ServerId == serverId && c.EventKey == eventKey);
        }
        public long Add (ServerEventSwitchAdd datum)
        {
            ServerEventSwitchModel add = datum.ConvertMap<ServerEventSwitchAdd, ServerEventSwitchModel>();
            add.Id = IdentityHelper.CreateId();
            this._Resource.Insert(add);
            return add.Id;
        }
        public void Delete (long id)
        {
            if (!this._Resource.Delete(a => a.Id == id))
            {
                throw new Exception("rpc.store.event.delete.fail");
            }
        }
        public ServerEventSwitchModel Get (long id)
        {
            ServerEventSwitchModel data = this._Resource.Get(a => a.Id == id);
            if (data == null)
            {
                throw new Exception("rpc.store.event.not.find");
            }
            return data;
        }
        public bool Update (ServerEventSwitchModel source, ServerEventSwitchSet set)
        {
            return this._Resource.Update(source, set);
        }

        public void SetIsEnable (long id, bool isEnable)
        {
            if (!this._Resource.Update(a => a.IsEnable == isEnable, a => a.Id == id))
            {
                throw new Exception("rpc.store.event.set.fail");
            }
        }
    }
}
