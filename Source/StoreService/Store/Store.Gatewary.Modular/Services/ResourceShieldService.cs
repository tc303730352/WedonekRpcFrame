using WeDonekRpc.Model;
using RpcStore.RemoteModel.ResourceShield;
using RpcStore.RemoteModel.ResourceShield.Model;
using Store.Gatewary.Modular.Interface;

namespace Store.Gatewary.Modular.Services
{
    internal class ResourceShieldService : IResourceShieldService
    {
        public void CancelResourceShieId (long resourceId)
        {
            new CancelResourceShieId
            {
                ResourceId = resourceId,
            }.Send();
        }

        public void CancelShieId (long id)
        {
            new CancelShieId
            {
                Id = id,
            }.Send();
        }

        public ResourceShieldDatum[] Query (ResourceShieldQuery query, IBasicPage paging, out int count)
        {
            return new QueryResourceShield
            {
                Query = query,
                Index = paging.Index,
                Size = paging.Size,
                NextId = paging.NextId,
                SortName = paging.SortName,
                IsDesc = paging.IsDesc
            }.Send(out count);
        }

        public void AddResourceShieId (ResourceShieldAdd datum)
        {
            new AddResourceShieId
            {
                Datum = datum
            }.Send();
        }

        public void AddShield (ShieldAddDatum datum)
        {
            new AddShield
            {
                Datum = datum,
            }.Send();
        }

    }
}
