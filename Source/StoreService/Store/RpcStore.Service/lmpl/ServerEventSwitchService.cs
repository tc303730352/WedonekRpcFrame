using WeDonekRpc.Client;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;
using WeDonekRpc.ModularModel.SysEvent.Msg;
using RpcStore.Collect;
using RpcStore.Model.ExtendDB;
using RpcStore.Model.ServerEventSwitch;
using RpcStore.RemoteModel.ServerEventSwitch.Model;
using RpcStore.RemoteModel.SysEventConfig.Model;
using RpcStore.Service.Interface;

namespace RpcStore.Service.lmpl
{
    internal class ServerEventSwitchService : IServerEventSwitchService
    {
        private readonly IServerEventSwitchCollect _Service;
        private readonly IServerCollect _Server;
        private readonly ISystemEventConfigCollect _SysEvent;

        public ServerEventSwitchService (IServerEventSwitchCollect service,
            IServerCollect server,
            ISystemEventConfigCollect sysEvent)
        {
            this._Server = server;
            this._Service = service;
            this._SysEvent = sysEvent;
        }

        public long Add (EventSwitchAdd add)
        {
            SystemEventConfig config = this._SysEvent.Get(add.SysEventId);
            ServerEventSwitchAdd datum = add.ConvertMap<EventSwitchAdd, ServerEventSwitchAdd>();
            datum.Module = config.Module;
            datum.EventKey = add.GetEventKey(add.SysEventId);
            return this._Service.Add(datum);
        }

        public void Delete (long id)
        {
            ServerEventSwitchModel model = this._Service.Get(id);
            this._Service.Delete(model);
        }

        public EventSwitchData Get (long id)
        {
            ServerEventSwitchModel model = this._Service.Get(id);
            EventSwitchData data = model.ConvertMap<ServerEventSwitchModel, EventSwitchData>();
            data.SysEventName = this._SysEvent.GetName(model.SysEventId);
            if (model.ServerId != 0)
            {
                data.ServerName = this._Server.GetName(model.ServerId);
            }
            return data;
        }

        public PagingResult<EventSwitch> Query (EventSwitchQuery query, IBasicPage paging)
        {
            ServerEventSwitch[] list = this._Service.Query(query, paging, out int count);
            if (list.IsNull())
            {
                return new PagingResult<EventSwitch>(count);
            }
            EventSwitch[] dtos = list.ConvertMap<ServerEventSwitch, EventSwitch>();
            Dictionary<int, string> evNames = this._SysEvent.GetName(dtos.Distinct(c => c.SysEventId));
            Dictionary<long, string> servers = this._Server.GetNames(list.Where(c => c.ServerId != 0).Select(c => c.ServerId).Distinct().ToArray());
            dtos.ForEach(c =>
            {
                c.SysEventName = evNames.GetValueOrDefault(c.SysEventId);
                if (c.ServerId != 0)
                {
                    c.ServerName = servers.GetValueOrDefault(c.ServerId);
                }
            });
            return new PagingResult<EventSwitch>(dtos, count);
        }

        public void SetIsEnable (long id, bool isEnable)
        {
            ServerEventSwitchModel model = this._Service.Get(id);
            if (this._Service.SetIsEnable(model, isEnable))
            {
                new RefreshRpcEvent
                {
                    RpcMerId = model.RpcMerId,
                    Module = model.Module
                }.Send();
            }
        }

        public bool Update (long id, EventSwitchSet set)
        {
            ServerEventSwitchModel model = this._Service.Get(id);
            ServerEventSwitchSet data = set.ConvertMap<EventSwitchSet, ServerEventSwitchSet>();
            data.EventKey = set.GetEventKey(model.SysEventId);
            return this._Service.Update(model, data);
        }
    }
}
