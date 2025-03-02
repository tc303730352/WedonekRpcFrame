using WeDonekRpc.Client;
using WeDonekRpc.Client.Interface;
using RpcStore.RemoteModel.ResourceShield;
using RpcStore.RemoteModel.ResourceShield.Model;
using RpcStore.Service.Interface;

namespace RpcStore.Service.Event
{
    internal class ResourceShieldEvent : IRpcApiService
    {
        private readonly IResourceShieldService _Service;
        public ResourceShieldEvent (IResourceShieldService service)
        {
            this._Service = service;
        }

        public void AddResourceShieId (AddResourceShieId param)
        {
            this._Service.SyncShieId(param.Datum);
        }
        public void AddShield (AddShield add)
        {
            this._Service.AddShield(add.Datum);
        }

        public void CancelResourceShieId (CancelResourceShieId obj)
        {
            this._Service.CancelResourceShieId(obj.ResourceId);
        }
        public void CancelShieId (CancelShieId obj)
        {
            this._Service.CancelShieId(obj.Id);
        }


        public PagingResult<ResourceShieldDatum> QueryResourceShield (QueryResourceShield query)
        {
            return this._Service.Query(query.Query, query.ToBasicPage());
        }

    }
}
