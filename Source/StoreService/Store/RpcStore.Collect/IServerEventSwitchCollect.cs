using WeDonekRpc.Model;
using RpcStore.Model.ExtendDB;
using RpcStore.Model.ServerEventSwitch;
using RpcStore.RemoteModel.ServerEventSwitch.Model;

namespace RpcStore.Collect
{
    public interface IServerEventSwitchCollect
    {
        long Add (ServerEventSwitchAdd add);
        void Delete (ServerEventSwitchModel source);
        ServerEventSwitchModel Get (long id);
        ServerEventSwitch[] Query (EventSwitchQuery query, IBasicPage paging, out int count);
        bool SetIsEnable (ServerEventSwitchModel model, bool isEnable);
        bool Update (ServerEventSwitchModel source, ServerEventSwitchSet set);
    }
}