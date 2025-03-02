using WeDonekRpc.Model;
using RpcStore.Model.DB;
using RpcStore.RemoteModel.ServerPublic.Model;

namespace RpcStore.DAL
{
    public interface IServerPublicSchemeDAL
    {
        long Add (PublicSchemeAdd add);
        void Enable (long id);
        void Disable (long id);
        void Delete (ServerPublicSchemeModel scheme);
        ServerPublicSchemeModel Get (long id);
        ServerPublicScheme[] Query (PublicSchemeQuery query, IBasicPage paging, out int count);
        void Set (ServerPublicSchemeModel scheme, PublicScheme set);
    }
}