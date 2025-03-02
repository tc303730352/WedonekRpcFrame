using RpcStore.RemoteModel.TransmitScheme;
using RpcStore.RemoteModel.TransmitScheme.Model;
using Store.Gatewary.Modular.Interface;
using WeDonekRpc.Client;
using WeDonekRpc.Model;

namespace Store.Gatewary.Modular.Services
{
    internal class TransmitSchemeService : ITransmitSchemeService
    {
        public long Add (TransmitSchemeAdd datum)
        {
            return new AddTransmitScheme
            {
                Datum = datum
            }.Send();
        }

        public void Delete (long id)
        {
            new DeleteTransmitScheme
            {
                Id = id
            }.Send();
        }
        public TransmitDatum[] Generate (TransmitGenerate param)
        {
            return new GenerateTransmit
            {
                Arg = param
            }.Send();
        }
        public PagingResult<TransmitScheme> Query (TransmitSchemeQuery query, IBasicPage paging)
        {
            TransmitScheme[] list = new QueryTransmitScheme
            {
                Query = query,
                Index = paging.Index,
                Size = paging.Size,
                NextId = paging.NextId,
                SortName = paging.SortName,
                IsDesc = paging.IsDesc
            }.Send(out int count);
            return new PagingResult<TransmitScheme>(list, count);
        }
        public void SetIsEnable (long id, bool enable)
        {
            new SetTransmitSchemeIsEnable
            {
                Id = id,
                IsEnable = enable
            }.Send();
        }
        public void Set (long id, TransmitSchemeSet datum)
        {
            new SetTransmitScheme
            {
                Datum = datum,
                Id = id
            }.Send();
        }
        public TransmitSchemeData Get (long id)
        {
            return new GetTransmitScheme
            {
                Id = id
            }.Send();
        }

        public void SetItem (long id, TransmitDatum[] items)
        {
            new SyncTransmitItem
            {
                Transmits = items,
                SchemeId = id
            }.Send();
        }

        public TransmitSchemeDetailed GetDetailed (long id)
        {
            return new GetTransmitDetailed()
            {
                Id = id
            }.Send();
        }
    }
}
