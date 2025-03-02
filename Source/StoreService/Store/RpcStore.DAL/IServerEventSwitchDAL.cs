using RpcStore.Model.ExtendDB;
using RpcStore.Model.ServerEventSwitch;
using RpcStore.RemoteModel.ServerEventSwitch.Model;
using WeDonekRpc.Model;

namespace RpcStore.DAL
{
    public interface IServerEventSwitchDAL
    {
        long Add (ServerEventSwitchAdd add);
        bool CheckIsRepeat (long rpcMerId, long serverId, string eventKey);
        void Delete (long id);
        ServerEventSwitchModel Get (long id);
        ServerEventSwitch[] Query (EventSwitchQuery query, IBasicPage paging, out int count);
        void SetIsEnable (long id, bool isEnable);
        bool Update (ServerEventSwitchModel source, ServerEventSwitchSet set);
    }
}