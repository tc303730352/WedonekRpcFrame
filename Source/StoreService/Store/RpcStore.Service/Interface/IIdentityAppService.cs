using WeDonekRpc.Client;
using WeDonekRpc.Model;
using RpcStore.RemoteModel.Identity.Model;

namespace RpcStore.Service.Interface
{
    public interface IIdentityAppService
    {
        long Add (IdentityDatum add);
        void Delete (long id);
        IdentityAppData Get (long id);
        PagingResult<IdentityApp> Query (IdentityQuery query, IBasicPage paging);
        void Set (long id, IdentityDatum param);
        void SetIsEnable (long id, bool isEnable);
    }
}