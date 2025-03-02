using WeDonekRpc.Client;
using WeDonekRpc.Model;
using RpcStore.RemoteModel.ServerEventSwitch.Model;

namespace RpcStore.Service.Interface
{
    public interface IServerEventSwitchService
    {
        long Add (EventSwitchAdd add);
        void Delete (long id);
        EventSwitchData Get (long id);
        PagingResult<EventSwitch> Query (EventSwitchQuery query, IBasicPage paging);
        void SetIsEnable (long id, bool isEnable);
        bool Update (long id, EventSwitchSet set);
    }
}