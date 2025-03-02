using WeDonekRpc.Model;
using RpcStore.Model.DB;
using RpcStore.RemoteModel.ServerPublic.Model;

namespace RpcStore.Collect
{
    public interface IServerPublicSchemeCollect
    {
        long[] CheckRepeat (long? schemeId, Dictionary<long, int> systemType);
        long Add (PublicSchemeAdd add);
        void Delete (ServerPublicSchemeModel scheme);
        bool Disable (ServerPublicSchemeModel scheme);
        bool Enable (ServerPublicSchemeModel scheme);
        ServerPublicSchemeModel Get (long id);
        PublicScheme GetDetailed (long id);
        ServerPublicScheme[] Query (PublicSchemeQuery query, IBasicPage paging, out int count);
        void Set (ServerPublicSchemeModel scheme, PublicScheme set);
    }
}