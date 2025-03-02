using WeDonekRpc.Model;
using RpcStore.RemoteModel.Mer;
using RpcStore.RemoteModel.Mer.Model;
using Store.Gatewary.Modular.Interface;

namespace Store.Gatewary.Modular.Services
{
    internal class MerService : IMerService
    {
        public long AddMer (RpcMerAdd datum)
        {
            return new AddMer
            {
                Datum = datum,
            }.Send();
        }

        public void DeleteMer (long rpcMerId)
        {
            new DeleteMer
            {
                RpcMerId = rpcMerId,
            }.Send();
        }
        public BasicRpcMer[] GetMerItems ()
        {
            return new GetMerItems().Send();
        }
        public RpcMerDatum GetMer (long rpcMerId)
        {
            return new GetMer
            {
                RpcMerId = rpcMerId,
            }.Send();
        }

        public RpcMer[] QueryMer (string name, IBasicPage paging, out int count)
        {
            return new QueryMer
            {
                Name = name,
                Index = paging.Index,
                Size = paging.Size,
                NextId = paging.NextId,
                SortName = paging.SortName,
                IsDesc = paging.IsDesc
            }.Send(out count);
        }

        public void SetMer (long rpcMerId, RpcMerSet datum)
        {
            new SetMer
            {
                RpcMerId = rpcMerId,
                Datum = datum,
            }.Send();
        }

    }
}
