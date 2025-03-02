using WeDonekRpc.Client;
using WeDonekRpc.Model;
using RpcStore.RemoteModel.ServerPublic.Model;

namespace RpcStore.Service.Interface
{
    public interface IPublicSchemeService
    {
        long Add (PublicSchemeAdd add);
        long[] CheckRepeat (long? schemeId, Dictionary<long, int> systemType);
        void Delete (long id);
        bool Disable (long id);
        bool Enable (long id);
        PublicScheme Get (long id);
        PagingResult<ServerPublicScheme> Query (PublicSchemeQuery query, IBasicPage paging);
        void Set (long id, PublicScheme set);
    }
}