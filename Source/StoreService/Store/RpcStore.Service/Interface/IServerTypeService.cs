using WeDonekRpc.Client;
using WeDonekRpc.Model;
using RpcStore.RemoteModel.ServerType.Model;

namespace RpcStore.Service.Interface
{
    public interface IServerTypeService
    {
        long Add (ServerTypeAdd add);
        void CheckIsRepeat (string typeVal);
        void Delete (long id);
        ServerType Get (long id);
        string GetName(string typeVal);
        ServerType[] Gets (long groupId);
        PagingResult<ServerTypeDatum> Query (ServerTypeQuery query, IBasicPage paging);
        void Set (long id, ServerTypeSet param);
    }
}