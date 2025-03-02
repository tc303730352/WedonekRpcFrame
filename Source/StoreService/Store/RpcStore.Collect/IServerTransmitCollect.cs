using RpcStore.Model.DB;
using RpcStore.RemoteModel.TransmitScheme.Model;

namespace RpcStore.Collect
{
    public interface IServerTransmitCollect
    {
        long[] CheckIsConfig (long schemeId, long[] serverId);
        ServerTransmitConfigModel Get (long schemeId, long serverId);
        ServerTransmitConfigModel[] Gets (long schemeId, long[] serverId);
        ServerTransmitConfigModel Get (long id);
        void Set (ServerTransmitConfigModel config, TransmitConfig[] configs);
        ServerTransmitConfigModel[] Gets (long schemeId);
        void Add (TransmitDatum add);
        void Delete (ServerTransmitConfigModel config);
    }
}