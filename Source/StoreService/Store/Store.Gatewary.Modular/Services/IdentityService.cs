using WeDonekRpc.Model;
using RpcStore.RemoteModel.Identity;
using RpcStore.RemoteModel.Identity.Model;
using Store.Gatewary.Modular.Interface;

namespace Store.Gatewary.Modular.Services
{
    internal class IdentityService : IIdentityService
    {
        public long AddIdentityApp (IdentityDatum datum)
        {
            return new AddIdentityApp
            {
                Datum = datum,
            }.Send();
        }

        public void DeleteIdentityApp (long id)
        {
            new DeleteIdentityApp
            {
                Id = id,
            }.Send();
        }

        public IdentityAppData GetIdentityApp (long id)
        {
            return new GetIdentityApp
            {
                Id = id,
            }.Send();
        }

        public IdentityApp[] QueryIdentity (IdentityQuery query, IBasicPage paging, out int count)
        {
            return new QueryIdentity
            {
                Query = query,
                Index = paging.Index,
                Size = paging.Size,
                NextId = paging.NextId,
                SortName = paging.SortName,
                IsDesc = paging.IsDesc
            }.Send(out count);
        }

        public void SetIdentity (long id, IdentityDatum datum)
        {
            new SetIdentity
            {
                Id = id,
                Datum = datum,
            }.Send();
        }

        public void SetIdentityIsEnable (long id, bool isEnable)
        {
            new SetIdentityIsEnable
            {
                Id = id,
                IsEnable = isEnable,
            }.Send();
        }

    }
}
