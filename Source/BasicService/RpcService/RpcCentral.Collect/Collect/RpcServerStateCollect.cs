using RpcCentral.Common;
using RpcCentral.DAL;
using RpcCentral.Model;
using RpcCentral.Model.DB;
using WeDonekRpc.CacheClient.Interface;
using WeDonekRpc.Helper;
using WeDonekRpc.Model.Server;

namespace RpcCentral.Collect.Collect
{
    internal class RpcServerStateCollect : IRpcServerStateCollect
    {
        private readonly IServerRunStateDAL _ServerRunState;
        private readonly ICacheController _Cache;
        public RpcServerStateCollect (IServerRunStateDAL serverRunState, ICacheController cache)
        {
            this._ServerRunState = serverRunState;
            this._Cache = cache;
        }

        public RunEnvironment GetEnvironments (long serverId)
        {
            string key = "Environment_" + serverId;
            if (this._Cache.TryGet(key, out RunEnvironment env))
            {
                return env;
            }
            env = this._ServerRunState.GetEnvironments(serverId);
            if (env != null)
            {
                _ = this._Cache.Set(key, env, new TimeSpan(1, 0, 0, 0));
            }
            return env;
        }
        private void _Clear (long serverId)
        {
            string key = "Environment_" + serverId;
            _ = this._Cache.Remove(key);
        }
        private void _AddRunState (long id, ProcessDatum datum)
        {
            ServerRunStateModel add = datum.ConvertMap<ProcessDatum, ServerRunStateModel>();
            add.ServerId = id;
            add.SyncTime = DateTime.Now;
            this._ServerRunState.AddRunState(add);
        }
        private void _SetRunState (long id, ProcessDatum datum)
        {
            if (!this._ServerRunState.UpdateRunState(id, datum))
            {
                throw new ErrorException("rpc.sever.runstate.set.error");
            }
            this._Clear(id);
        }
        public void SyncRunState (long id, ProcessDatum datum)
        {
            if (!this._ServerRunState.CheckIsReg(id))
            {
                this._AddRunState(id, datum);
                return;
            }
            this._SetRunState(id, datum);
        }
    }
}
