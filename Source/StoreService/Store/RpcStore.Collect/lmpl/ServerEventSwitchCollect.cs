using RpcStore.DAL;
using RpcStore.Model.ExtendDB;
using RpcStore.Model.ServerEventSwitch;
using RpcStore.RemoteModel.ServerEventSwitch.Model;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;

namespace RpcStore.Collect.lmpl
{
    internal class ServerEventSwitchCollect : IServerEventSwitchCollect
    {
        private readonly IServerEventSwitchDAL _BasicDAL;

        public ServerEventSwitchCollect (IServerEventSwitchDAL basicDAL)
        {
            this._BasicDAL = basicDAL;
        }

        public long Add (ServerEventSwitchAdd add)
        {
            if (this._BasicDAL.CheckIsRepeat(add.RpcMerId, add.ServerId, add.EventKey))
            {
                throw new ErrorException("rpc.store.event.repeat");
            }
            return this._BasicDAL.Add(add);
        }

        public void Delete (ServerEventSwitchModel source)
        {
            if (source.IsEnable)
            {
                throw new ErrorException("rpc.store.event.already.enable");
            }
            this._BasicDAL.Delete(source.Id);
        }

        public ServerEventSwitchModel Get (long id)
        {
            return this._BasicDAL.Get(id);
        }

        public ServerEventSwitch[] Query (EventSwitchQuery query, IBasicPage paging, out int count)
        {
            return this._BasicDAL.Query(query, paging, out count);
        }

        public bool SetIsEnable (ServerEventSwitchModel model, bool isEnable)
        {
            if (model.IsEnable == isEnable)
            {
                return false;
            }
            this._BasicDAL.SetIsEnable(model.Id, isEnable);
            return true;
        }

        public bool Update (ServerEventSwitchModel source, ServerEventSwitchSet set)
        {
            return this._BasicDAL.Update(source, set);
        }
    }
}
