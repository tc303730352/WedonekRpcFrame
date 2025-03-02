using WeDonekRpc.Model;
using RpcStore.Model.DB;

namespace RpcStore.Collect
{
    public interface IServerRunStateCollect
    {
        ServerRunStateModel Get(long serverId);
        ServerRunStateModel[] Query(IBasicPage paging, out int count);
    }
}