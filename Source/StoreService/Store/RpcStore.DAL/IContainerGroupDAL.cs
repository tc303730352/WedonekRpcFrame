using RpcStore.Model.ContainerGroup;
using RpcStore.Model.DB;
using RpcStore.RemoteModel.ContainerGroup.Model;
using WeDonekRpc.Model;

namespace RpcStore.DAL
{
    public interface IContainerGroupDAL
    {
        void Set (long id, ContainerGroupSet set);
        long Add (ContainerGroupAdd data);
        ContainerGroupModel[] Query (ContainerGroupQuery query, IBasicPage paging, out int count);
        void Delete (long id);
        bool CheckHostMac (string number, ContainerType containerType);
        bool CheckName (string name);
        ContainerGroupModel Get (long id);
        ContainerGroupDto[] Gets (int? regionId);
        Dictionary<long, string> GetNames (long[] ids);
        ContainerGroupDto[] Gets (long[] ids);
        string GetName (long id);
    }
}