using RpcStore.Model.DB;
using RpcStore.Model.TransmitScheme;
using RpcStore.RemoteModel.TransmitScheme.Model;
using WeDonekRpc.Model;

namespace RpcStore.Collect
{
    public interface IServerTransmitSchemeCollect
    {
        ServerTransmitScheme GetScheme (long id);
        long Add (TransmitSchemeAdd scheme);
        void Delete (ServerTransmitSchemeModel scheme);
        ServerTransmitSchemeModel Get (long id);
        ServerTransmitSchemeModel[] Query (TransmitSchemeQuery query, IBasicPage paging, out int count);
        bool SetIsEnable (ServerTransmitSchemeModel scheme, bool isEnable);
        void SetScheme (ServerTransmitSchemeModel scheme, TransmitSchemeSet set);
        void SyncItem (long schemeId, TransmitDatum[] transmits);
    }
}