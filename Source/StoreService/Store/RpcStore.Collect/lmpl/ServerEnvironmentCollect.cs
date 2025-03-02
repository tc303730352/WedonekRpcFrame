using RpcStore.DAL;
using RpcStore.Model.DB;

namespace RpcStore.Collect.lmpl
{
    internal class ServerEnvironmentCollect
    {
        private readonly IServerEnvironmentDAL _BasicDAL;

        public ServerEnvironmentCollect (IServerEnvironmentDAL basicDAL)
        {
            this._BasicDAL = basicDAL;
        }
        public ServerEnvironmentModel Get (long serverId)
        {
            return this._BasicDAL.Get(serverId);
        }
    }
}
