using WeDonekRpc.Model;
using RpcStore.RemoteModel.ServerGroup.Model;

namespace RpcStore.Service.Interface
{
    public interface IServerGroupService
    {
        long AddGroup (ServerGroupAdd add);
        void CheckIsRepeat (string typeVal);
        void Delete (long id);
        ServerGroupDatum GetGroup (long id);
        ServerGroupItem[] GetGroups ();
        ServerGroupList[] GetList (RpcServerType? serverType);
        void SetGroup (long id, string name);
    }
}