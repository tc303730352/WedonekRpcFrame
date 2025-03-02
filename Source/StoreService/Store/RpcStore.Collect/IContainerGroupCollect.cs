using RpcStore.Model.ContainerGroup;
using RpcStore.Model.DB;
using RpcStore.RemoteModel.ContainerGroup.Model;
using WeDonekRpc.Model;

namespace RpcStore.Collect
{
    public interface IContainerGroupCollect
    {
        ContainerGroupModel[] Query (ContainerGroupQuery query, IBasicPage paging, out int count);
        void Delete (ContainerGroupModel source);
        long Add (ContainerGroupAdd data);
        void Set (ContainerGroupModel source, ContainerGroupSet set);
        ContainerGroupModel Get (long id);
        ContainerGroupDto[] Gets (int? regionId);
        Dictionary<long, string> GetNames (long[] ids);
        ContainerGroupDto[] Gets (long[] ids);
        string GetName (long id);
    }
}