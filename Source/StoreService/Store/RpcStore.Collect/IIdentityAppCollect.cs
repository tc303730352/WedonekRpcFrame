using WeDonekRpc.Model;
using RpcStore.Model.ExtendDB;
using RpcStore.RemoteModel.Identity.Model;

namespace RpcStore.Collect
{
    public interface IIdentityAppCollect
    {
        long Add (IdentityDatum add);
        void Delete (IdentityAppModel app);
        IdentityAppModel Get (long id);
        IdentityApp[] Query (IdentityQuery query, IBasicPage paging, out int count);
        bool Set (IdentityAppModel app, IdentityDatum param);
        bool SetIsEnable (IdentityAppModel app, bool isEnable);
    }
}