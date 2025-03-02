using WeDonekRpc.Client;
using WeDonekRpc.Model;
using RpcStore.RemoteModel.ServerPublic.Model;

namespace Store.Gatewary.Modular.Interface
{
    public interface IPublicSchemeService
    {
        long Add(PublicSchemeAdd add);

        void Delete(long id);

        PublicScheme Get(long id);

        PagingResult<ServerPublicScheme> Query(PublicSchemeQuery query, IBasicPage paging);

        void Set(long id, PublicScheme data);

        bool SetIsEnable(long id, bool isEnable);
    }
}