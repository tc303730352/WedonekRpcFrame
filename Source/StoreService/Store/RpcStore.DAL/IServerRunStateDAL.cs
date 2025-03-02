using WeDonekRpc.Model;
using RpcStore.Model.DB;

namespace RpcStore.DAL
{
    public interface IServerRunStateDAL
    {
        void Delete(long serverId);
        ServerRunStateModel Get(long serverId);
        ServerRunStateModel[] Query(IBasicPage paging, out int count);
    }
}