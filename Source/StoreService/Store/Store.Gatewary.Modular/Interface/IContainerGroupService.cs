using RpcStore.RemoteModel.ContainerGroup.Model;
using WeDonekRpc.Client;
using WeDonekRpc.Model;

namespace Store.Gatewary.Modular.Interface
{
    public interface IContainerGroupService
    {
        int Add (ContainerGroupAdd data);
        void Delete (long id);
        ContainerGroupDatum Get (long id);
        ContainerGroupItem[] GetItems (int? regionId);
        PagingResult<ContainerGroup> Query (ContainerGroupQuery query, IBasicPage paging);
        void Set (long id, ContainerGroupSet set);
    }
}