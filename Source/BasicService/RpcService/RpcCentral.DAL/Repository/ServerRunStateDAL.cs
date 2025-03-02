using RpcCentral.Common;
using RpcCentral.Model;
using RpcCentral.Model.DB;
using WeDonekRpc.Model.Server;
using WeDonekRpc.SqlSugar;

namespace RpcCentral.DAL.Repository
{
    internal class ServerRunStateDAL : IServerRunStateDAL
    {
        private readonly IRepository<ServerRunStateModel> _Db;
        public ServerRunStateDAL (IRepository<ServerRunStateModel> db)
        {
            this._Db = db;
        }
        public bool SetServiceState (ServiceRunState[] states)
        {
            ServerRunStateModel[] models = states.ConvertMap<ServiceRunState, ServerRunStateModel>();
            return this._Db.UpdateOnly(models, a => new
            {
                a.ConNum,
                a.CpuRunTime,
                a.CpuRate,
                a.GCBody,
                a.LockContentionCount,
                a.SyncTime,
                a.ThreadNum,
                a.WorkMemory,
                a.TimerNum,
                a.ThreadPool
            });
        }
        public RunEnvironment GetEnvironments (long serverId)
        {
            return this._Db.Get<RunEnvironment>(a => a.ServerId == serverId);
        }
        public bool CheckIsReg (long id)
        {
            return this._Db.IsExist(c => c.ServerId == id);
        }
        public void AddRunState (ServerRunStateModel add)
        {
            this._Db.Insert(add);
        }
        public bool UpdateRunState (long serverId, ProcessDatum datum)
        {
            return this._Db.Update((a) => new ServerRunStateModel
            {
                OSArchitecture = datum.OSArchitecture,
                StartTime = datum.StartTime,
                OSDescription = datum.OSDescription,
                OSType = datum.OSType,
                Framework = datum.Framework,
                WorkMemory = datum.WorkMemory,
                RunUserIdentity = datum.RunUserIdentity,
                RunIsAdmin = datum.RunIsAdmin,
                IsLittleEndian = datum.IsLittleEndian,
                ProcessorCount = datum.ProcessorCount,
                SystemStartTime = datum.SystemStartTime,
                RuntimeIdentifier = datum.RuntimeIdentifier,
                RunUserGroups = datum.RunUserGroups,
                MachineName = datum.MachineName,
                PName = datum.PName,
                Pid = datum.Pid,
                ProcessArchitecture = datum.ProcessArchitecture
            }, a => a.ServerId == serverId);
        }
    }
}
