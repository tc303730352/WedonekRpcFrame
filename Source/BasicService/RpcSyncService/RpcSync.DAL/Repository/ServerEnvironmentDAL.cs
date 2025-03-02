using RpcSync.Model.DB;
using WeDonekRpc.Client;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.IdGenerator;
using WeDonekRpc.Model.Server;
using WeDonekRpc.SqlSugar;

namespace RpcSync.DAL.Repository
{
    internal class ServerEnvironmentDAL : IServerEnvironmentDAL
    {
        private readonly IRepository<ServerEnvironmentModel> _Db;
        public ServerEnvironmentDAL (IRepository<ServerEnvironmentModel> db)
        {
            this._Db = db;
        }
        public ServerEnvironmentModel Get (long serverId)
        {
            return this._Db.Get(a => a.ServerId == serverId);
        }
        public void SetModules (long id, ProcModule[] modules)
        {
            if (!this._Db.Update(a => new ServerEnvironmentModel
            {
                Modules = modules,
                SyncTime = DateTime.Now
            }, a => a.Id == id))
            {
                throw new ErrorException("rpc.sync.environment.set.fail");
            }
        }
        public void Add (long serverId, EnvironmentConfig config)
        {
            ServerEnvironmentModel add = config.ConvertMap<EnvironmentConfig, ServerEnvironmentModel>();
            add.ServerId = serverId;
            add.Id = IdentityHelper.CreateId();
            add.SyncTime = DateTime.Now;
            this._Db.Insert(add);
        }
        public bool Set (ServerEnvironmentModel model, EnvironmentConfig set)
        {
            string[] cols = model.Merge(set);
            if (cols.Length == 0)
            {
                return false;
            }
            cols = cols.Add("SyncTime");
            model.SyncTime = DateTime.Now;
            if (!this._Db.Update(model, cols))
            {
                throw new ErrorException("rpc.sync.environment.set.fail");
            }
            return true;
        }

        public long GetId (long serverId)
        {
            return this._Db.Get(a => a.ServerId == serverId, a => a.Id);
        }
    }
}
