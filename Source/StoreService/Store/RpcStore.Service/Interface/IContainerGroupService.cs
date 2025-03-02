using RpcStore.RemoteModel.ContainerGroup.Model;
using WeDonekRpc.Client;
using WeDonekRpc.Model;

namespace RpcStore.Service.Interface
{
    public interface IContainerGroupService
    {
        PagingResult<ContainerGroup> Query (ContainerGroupQuery query, IBasicPage paging);
        long Add (ContainerGroupAdd add);
        void Delete (long id);

        void Set (long id, ContainerGroupSet set);
        ContainerGroupItem[] GetItems (int? regionId);
        ContainerGroupDatum Get (long id);
    }
}