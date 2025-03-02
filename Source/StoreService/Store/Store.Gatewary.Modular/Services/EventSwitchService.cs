using WeDonekRpc.HttpApiGateway.Model;
using WeDonekRpc.Client;
using RpcStore.RemoteModel.ServerEventSwitch;
using RpcStore.RemoteModel.ServerEventSwitch.Model;
using Store.Gatewary.Modular.Interface;

namespace Store.Gatewary.Modular.Services
{
    internal class EventSwitchService : IEventSwitchService
    {
        public long Add (EventSwitchAdd datum)
        {
            return new AddEventSwitch
            {
                Datum = datum
            }.Send();
        }
        public void Delete (long id)
        {
            new DeleteEventSwitch
            {
                Id = id
            }.Send();
        }

        public EventSwitchData Get (long id)
        {
            return new GetEventSwitch
            {
                Id = id
            }.Send();
        }
        public PagingResult<EventSwitch> Query (PagingParam<EventSwitchQuery> param)
        {
            return new QueryEventSwitch
            {
                Query = param.Query,
                Index = param.Index,
                Size = param.Size,
                SortName = param.SortName,
                IsDesc = param.IsDesc,
                NextId = param.NextId
            }.Send();
        }

        public void SetIsEnable (long id, bool isEnable)
        {
            new SetEventSwitchIsEnable
            {
                Id = id,
                IsEnable = isEnable
            }.Send();
        }

        public bool Update (long id, EventSwitchSet set)
        {
            return new SetEventSwitch
            {
                Id = id,
                Datum = set
            }.Send();
        }
    }
}
