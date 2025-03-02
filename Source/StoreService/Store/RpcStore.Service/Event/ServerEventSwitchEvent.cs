using WeDonekRpc.Client;
using WeDonekRpc.Client.Interface;
using RpcStore.RemoteModel.ServerEventSwitch;
using RpcStore.RemoteModel.ServerEventSwitch.Model;
using RpcStore.Service.Interface;

namespace RpcStore.Service.Event
{
    internal class ServerEventSwitchEvent : IRpcApiService
    {
        private readonly IServerEventSwitchService _Service;

        public ServerEventSwitchEvent (IServerEventSwitchService service)
        {
            this._Service = service;
        }
        public void SetEventSwitchIsEnable (SetEventSwitchIsEnable obj)
        {
            this._Service.SetIsEnable(obj.Id, obj.IsEnable);
        }
        public EventSwitchData GetEventSwitch (GetEventSwitch obj)
        {
            return this._Service.Get(obj.Id);
        }
        public bool SetEventSwitch (SetEventSwitch obj)
        {
            return this._Service.Update(obj.Id, obj.Datum);
        }
        public long AddEventSwitch (AddEventSwitch obj)
        {
            return this._Service.Add(obj.Datum);
        }
        public void DeleteEventSwitch (DeleteEventSwitch obj)
        {
            this._Service.Delete(obj.Id);
        }
        public PagingResult<EventSwitch> QueryEventSwitch (QueryEventSwitch obj)
        {
            return this._Service.Query(obj.Query, obj.ToBasicPage());
        }
    }
}
