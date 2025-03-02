using WeDonekRpc.Model;
using RpcStore.Model.DB;

namespace RpcStore.DAL
{
    public interface IServerGroupDAL
    {
        long Add(ServerGroupModel datum);
        bool CheckIsRepeat(string typeVal);
        void Delete(long id);
        ServerGroupModel Get(long id);
        Dictionary<long, string> GetGroupName(long[] ids);
        string GetName(long id);
        ServerGroupModel[] Gets();
        ServerGroupModel[] Gets(long[] ids);
        ServerGroupModel[] Query(string name, IBasicPage paging, out int count);
        void Set(long id, string name);
    }
}