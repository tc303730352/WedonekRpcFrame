using RpcStore.Model.DB;

namespace RpcStore.DAL
{
    public interface IServerEnvironmentDAL
    {
        void Clear (long serverId);
        ServerEnvironmentModel Get (long serverId);
    }
}