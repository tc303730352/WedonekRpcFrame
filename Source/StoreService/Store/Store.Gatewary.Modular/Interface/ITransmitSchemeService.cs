using RpcStore.RemoteModel.TransmitScheme.Model;
using WeDonekRpc.Client;
using WeDonekRpc.Model;

namespace Store.Gatewary.Modular.Interface
{
    public interface ITransmitSchemeService
    {
        TransmitDatum[] Generate (TransmitGenerate param);
        long Add (TransmitSchemeAdd datum);
        void Delete (long id);
        TransmitSchemeData Get (long id);
        PagingResult<TransmitScheme> Query (TransmitSchemeQuery query, IBasicPage paging);
        void Set (long id, TransmitSchemeSet datum);
        void SetIsEnable (long id, bool enable);
        void SetItem (long id, TransmitDatum[] value);
        TransmitSchemeDetailed GetDetailed (long id);
    }
}