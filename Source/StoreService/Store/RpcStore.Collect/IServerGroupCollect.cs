using WeDonekRpc.Model;
using RpcStore.Model.DB;
using RpcStore.RemoteModel.ServerGroup.Model;

namespace RpcStore.Collect
{
    public interface IServerGroupCollect
    {
        string GetName(long id);
        long AddGroup(ServerGroupAdd group);
        void CheckIsRepeat(string typeVal);
        void Delete(ServerGroupModel group);
        ServerGroupModel GetGroup(long id);
        ServerGroupModel[] GetGroup(long[] ids);

        Dictionary<long,string> GetGroupName(long[] ids);
        ServerGroupModel[] GetGroups();
        ServerGroupModel[] QueryGroup(string name, IBasicPage paging, out int count);
        void SetGroup(ServerGroupModel group, string name);
    }
}