using RpcStore.Model.DB;

namespace RpcStore.Collect
{
    public interface IServerCurConfigCollect
    {
        ServerCurConfigModel GetConfig (long serverId);
    }
}