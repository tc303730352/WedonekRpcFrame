using WeDonekRpc.Client;
using WeDonekRpc.Client.Interface;
using RpcStore.RemoteModel.ResourceModular;
using RpcStore.RemoteModel.ResourceModular.Model;
using RpcStore.Service.Interface;

namespace RpcStore.Service.Event
{
    internal class ResourceModularEvent : IRpcApiService
    {
        private IResourceModularService _Service;
        public ResourceModularEvent(IResourceModularService service)
        {
            _Service = service;
        }

        public BasicModular[] GetBasicModular(GetBasicModular obj)
        {
            return _Service.Gets(obj.RpcMerId, obj.SystemType);
        }

        public PagingResult<ResourceModularDatum> QueryModular(QueryModular query)
        {
            return _Service.Query(query.Query, query.ToBasicPage());
        }

        public void SetModularRemark(SetModularRemark obj)
        {
            _Service.SetRemark(obj.Id, obj.Remark);
        }
    }
}
