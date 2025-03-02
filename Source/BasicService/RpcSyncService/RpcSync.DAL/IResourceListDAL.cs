using RpcSync.Model;
using RpcSync.Model.DB;

namespace RpcSync.DAL
{
    public interface IResourceListDAL
    {
        void Adds (ResourceListModel[] adds);
        long[] ClearResource ();
        InvalidResource[] GetInvalidResource ();
        ResourceData[] Gets (long modularId);
        void SetInvalid (InvalidResource[] invalids);
        void Sync (ResourceListModel[] adds, ResourceListModel[] sets);
    }
}