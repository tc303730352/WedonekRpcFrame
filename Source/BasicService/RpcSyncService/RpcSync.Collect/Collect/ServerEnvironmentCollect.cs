using WeDonekRpc.Model.Server;
using RpcSync.DAL;
using RpcSync.Model.DB;

namespace RpcSync.Collect.Collect
{
    internal class ServerEnvironmentCollect : IServerEnvironmentCollect
    {
        private readonly IServerEnvironmentDAL _BasicDAL;

        public ServerEnvironmentCollect (IServerEnvironmentDAL basicDAL)
        {
            this._BasicDAL = basicDAL;
        }
        public void SetModules (long serverId, ProcModule[] modules)
        {
            long id = this._BasicDAL.GetId(serverId);
            if (id != 0)
            {
                this._BasicDAL.SetModules(id, modules);
            }
        }
        public void Sync (long serverId, EnvironmentConfig config)
        {
            ServerEnvironmentModel source = this._BasicDAL.Get(serverId);
            if (source == null)
            {
                this._BasicDAL.Add(serverId, config);
            }
            else
            {
                _ = this._BasicDAL.Set(source, config);
            }
        }

    }
}
