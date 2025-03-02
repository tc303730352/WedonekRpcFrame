using RpcStore.RemoteModel.ContainerGroup;
using RpcStore.RemoteModel.ContainerGroup.Model;
using Store.Gatewary.Modular.Interface;
using WeDonekRpc.Client;
using WeDonekRpc.Model;

namespace Store.Gatewary.Modular.Services
{
    internal class ContainerGroupService : IContainerGroupService
    {
        public ContainerGroupItem[] GetItems (int? regionId)
        {
            return new GetsContainerGroup
            {
                RegionId = regionId
            }.Send();
        }
        public void Delete (long id)
        {
            new DeleteContainerGroup
            {
                Id = id
            }.Send();
        }
        public PagingResult<ContainerGroup> Query (ContainerGroupQuery query, IBasicPage paging)
        {
            ContainerGroup[] list = new QueryContainerGroup
            {
                Index = paging.Index,
                Size = paging.Size,
                SortName = paging.SortName,
                IsDesc = paging.IsDesc,
                NextId = paging.NextId,
                Query = query
            }.Send(out int count);
            return new PagingResult<ContainerGroup>(count, list);
        }
        public ContainerGroupDatum Get (long id)
        {
            return new GetContainerGroup
            {
                Id = id
            }.Send();
        }
        public int Add (ContainerGroupAdd data)
        {
            return new AddContainerGroup
            {
                Datum = data
            }.Send();
        }

        public void Set (long id, ContainerGroupSet set)
        {
            new SetContainerGroup
            {
                Datum = set,
                Id = id
            }.Send();
        }

    }
}
