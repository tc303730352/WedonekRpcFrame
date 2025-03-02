using WeDonekRpc.Client;
using WeDonekRpc.Model;
using RpcStore.RemoteModel.ResourceShield.Model;

namespace RpcStore.Service.Interface
{
    public interface IResourceShieldService
    {
        void SyncShieId (ResourceShieldAdd param);
        void CancelResourceShieId (long resourceId);
        void CancelShieId (long id);
        PagingResult<ResourceShieldDatum> Query (ResourceShieldQuery query, IBasicPage paging);
        void AddShield (ShieldAddDatum datum);
    }
}