using WeDonekRpc.Client;
using WeDonekRpc.Model;
using RpcStore.RemoteModel.RunState.Model;

namespace RpcStore.Service.Interface
{
    public interface IServerRunStateService
    {
        ServerRunState Get(long serverId);
        PagingResult<RunState> Query(IBasicPage paging);
    }
}