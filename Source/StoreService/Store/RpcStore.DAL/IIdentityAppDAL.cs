using WeDonekRpc.Model;
using RpcStore.Model.ExtendDB;
using RpcStore.RemoteModel.Identity.Model;

namespace RpcStore.DAL
{
    public interface IIdentityAppDAL
    {
        long Add (IdentityAppModel add);
        void Delete (long id);
        IdentityAppModel Get (long id);
        IdentityApp[] Query (IdentityQuery query, IBasicPage paging, out int count);
        void Set (long id, IdentityDatum param);
        void SetIsEnable (long id, bool isEnable);
    }
}