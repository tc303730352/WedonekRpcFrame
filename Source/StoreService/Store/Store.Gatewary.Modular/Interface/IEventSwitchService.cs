using WeDonekRpc.HttpApiGateway.Model;
using WeDonekRpc.Client;
using RpcStore.RemoteModel.ServerEventSwitch.Model;

namespace Store.Gatewary.Modular.Interface
{
    public interface IEventSwitchService
    {
        long Add (EventSwitchAdd datum);
        void Delete (long id);
        EventSwitchData Get (long id);
        PagingResult<EventSwitch> Query (PagingParam<EventSwitchQuery> param);
        void SetIsEnable (long id, bool isEnable);
        bool Update (long id, EventSwitchSet set);
    }
}