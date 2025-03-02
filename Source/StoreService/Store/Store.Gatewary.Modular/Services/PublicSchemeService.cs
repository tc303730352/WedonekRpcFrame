using WeDonekRpc.Client;
using WeDonekRpc.Model;
using RpcStore.RemoteModel.ServerPublic;
using RpcStore.RemoteModel.ServerPublic.Model;
using Store.Gatewary.Modular.Interface;

namespace Store.Gatewary.Modular.Services
{
    internal class PublicSchemeService : IPublicSchemeService
    {
        public void Delete(long id)
        {
            new DeletePublicScheme
            {
                Id = id
            }.Send();
        }
        public bool SetIsEnable(long id, bool isEnable)
        {
            if (isEnable)
            {
                return new EnablePublicScheme { Id = id }.Send();
            }
            else
            {
                return new DisablePublicScheme { Id = id }.Send();
            }
        }
        public PagingResult<ServerPublicScheme> Query(PublicSchemeQuery query, IBasicPage paging)
        {
            return new QueryPublicScheme
            {
                Index = paging.Index,
                Size = paging.Size,
                IsDesc = paging.IsDesc,
                SortName = paging.SortName,
                NextId = paging.NextId,
                Query = query
            }.Send();
        }
        public PublicScheme Get(long id)
        {
            return new GetPublicScheme
            {
                Id = id
            }.Send();
        }
        public void Set(long id, PublicScheme data)
        {
            new SetPublicScheme
            {
                Id = id,
                Scheme = data
            }.Send();
        }
        public long Add(PublicSchemeAdd add)
        {
            return new AddPublicScheme
            {
                SchemeAdd = add
            }.Send();
        }
    }
}
