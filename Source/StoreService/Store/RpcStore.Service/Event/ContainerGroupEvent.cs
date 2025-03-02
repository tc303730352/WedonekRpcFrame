using RpcStore.RemoteModel.ContainerGroup;
using RpcStore.RemoteModel.ContainerGroup.Model;
using RpcStore.Service.Interface;
using WeDonekRpc.Client;
using WeDonekRpc.Client.Interface;

namespace RpcStore.Service.Event
{
    internal class ContainerGroupEvent : IRpcApiService
    {
        private readonly IContainerGroupService _Service;

        public ContainerGroupEvent (IContainerGroupService service)
        {
            this._Service = service;
        }

        public void DeleteContainerGroup (DeleteContainerGroup obj)
        {
            this._Service.Delete(obj.Id);
        }
        public ContainerGroupDatum GetContainerGroup (GetContainerGroup obj)
        {
            return this._Service.Get(obj.Id);
        }
        public PagingResult<ContainerGroup> QueryContainerGroup (QueryContainerGroup obj)
        {
            return this._Service.Query(obj.Query, obj.ToBasicPage());
        }
        public void SetContainerGroup (SetContainerGroup obj)
        {
            this._Service.Set(obj.Id, obj.Datum);
        }
        public void AddContainerGroup (AddContainerGroup obj)
        {
            _ = this._Service.Add(obj.Datum);
        }
        public ContainerGroupItem[] GetsContainerGroup (GetsContainerGroup obj)
        {
            return this._Service.GetItems(obj.RegionId);
        }
    }
}
