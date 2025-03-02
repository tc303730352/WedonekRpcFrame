using RpcCentral.Model;
using RpcCentral.Model.DB;
using WeDonekRpc.Model;
using WeDonekRpc.SqlSugar;

namespace RpcCentral.DAL.Repository
{
    internal class RemoteServerGroupDAL : IRemoteServerGroupDAL
    {
        private readonly IRepository<RemoteServerGroup> _Db;
        public RemoteServerGroupDAL (IRepository<RemoteServerGroup> db)
        {
            this._Db = db;
        }
        public long[] GetRpcMer (long systemTypeId)
        {
            return this._Db.Gets(c => c.SystemType == systemTypeId, c => c.RpcMerId);
        }
        public RemoteConfig[] GetRemoteServer (long rpcMerId, long systemTypeId)
        {
            return this._Db.Gets<RemoteConfig>(c => c.RpcMerId == rpcMerId && c.SystemType == systemTypeId, "Weight");
        }
        public long[] GetRemoteServerId (long rpcMerId, long systemTypeId)
        {
            return this._Db.Join<RemoteServerConfig, long>((a, b) => a.ServerId == b.Id,
                (a, b) => a.RpcMerId == rpcMerId &&
                a.SystemType == systemTypeId &&
                b.ServiceState == RpcServiceState.正常 &&
                b.IsOnline, (a, b) => a.ServerId);
        }
        public RemoteServerGroup Find (long rpcMerId, long serverId)
        {
            return this._Db.Get(c => c.RpcMerId == rpcMerId && c.ServerId == serverId);
        }
    }
}
