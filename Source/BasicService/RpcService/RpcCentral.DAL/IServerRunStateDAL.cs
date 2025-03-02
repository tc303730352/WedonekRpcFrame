using RpcCentral.Model;
using RpcCentral.Model.DB;
using WeDonekRpc.Model.Server;

namespace RpcCentral.DAL
{
    public interface IServerRunStateDAL
    {
        RunEnvironment GetEnvironments (long serverId);
        void AddRunState (ServerRunStateModel add);
        bool CheckIsReg (long id);
        bool SetServiceState (ServiceRunState[] states);
        bool UpdateRunState (long serverId, ProcessDatum datum);
    }
}