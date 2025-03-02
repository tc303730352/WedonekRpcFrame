using WeDonekRpc.Client;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;
using RpcStore.Collect;
using RpcStore.Model.ExtendDB;
using RpcStore.Model.ResourceShield;
using RpcStore.RemoteModel.Resource.Model;
using RpcStore.Service.Interface;

namespace RpcStore.Service.lmpl
{
    internal class ResourceService : IResourceService
    {
        private readonly IResourceCollect _Resource;
        private readonly IServerTypeCollect _ServerType;
        private readonly IResourceModularService _Modular;
        private readonly IResourceShieldCollect _ShieId;
        public ResourceService (IResourceCollect resource,
            IResourceShieldCollect shield,
            IServerTypeCollect serverType,
            IResourceModularService modular)
        {
            this._ServerType = serverType;
            this._ShieId = shield;
            this._Resource = resource;
            this._Modular = modular;
        }

        public ResourceDto Get (long id)
        {
            ResourceListModel resource = this._Resource.Get(id);
            ResourceModularModel modular = this._Modular.Get(resource.ModularId);
            ResourceDto dto = resource.ConvertMap<ResourceListModel, ResourceDto>();
            dto.RpcMerId = modular.RpcMerId;
            dto.SystemType = modular.SystemType;
            dto.SystemTypeId = this._ServerType.GetIdByTypeVal(modular.SystemType);
            return dto;
        }
        public PagingResult<ResourceDatum> Query (ResourceQuery query, IBasicPage paging)
        {
            ResourceListModel[] list = this._Resource.Query(query, paging, out int count);
            if (list.Length == 0)
            {
                return null;
            }
            ResourceShieldState[] shieId = this._ShieId.CheckIsShieId(list.ConvertAll(a => a.Id));
            long now = DateTime.Now.ToLong();
            ResourceDatum[] datas = list.ConvertMap<ResourceListModel, ResourceDatum>((a, b) =>
            {
                ResourceShieldState state = shieId.Find(c => c.ResourceId == a.Id);
                if (state != null)
                {
                    b.IsShield = true;
                    if (state.BeOverdueTime != 0)
                    {
                        b.ShieldEndTime = (int)( state.BeOverdueTime - now );
                        if (b.ShieldEndTime > 0)
                        {
                            b.IsShield = true;
                        }
                    }
                    else
                    {
                        b.IsShield = true;
                    }
                }
                b.VerNumStr = a.VerNum.FormatVerNum();
                return b;
            });
            return new PagingResult<ResourceDatum>(datas, count);
        }
    }
}
