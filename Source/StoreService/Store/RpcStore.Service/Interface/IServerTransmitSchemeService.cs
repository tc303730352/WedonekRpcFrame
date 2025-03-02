using WeDonekRpc.Client;
using WeDonekRpc.Model;
using RpcStore.RemoteModel.TransmitScheme.Model;

namespace RpcStore.Service.Interface
{
    public interface IServerTransmitSchemeService
    {
        void SetIsEnable (long id, bool isEnable);
        long Add (TransmitSchemeAdd datum);
        void Delete (long id);
        TransmitSchemeData Get (long id);
        PagingResult<TransmitScheme> Query (TransmitSchemeQuery query, IBasicPage paging);
        void SetScheme (long id, TransmitSchemeSet set);
        void SyncItem (long schemeId, TransmitDatum[] transmits);
        TransmitSchemeDetailed GetDetailed (long id);
    }
}